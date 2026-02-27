namespace Base.Extensions.MessageBus.RabbitMQ;

public sealed class RabbitMqReceiveMessageBus : IReceiveMessageBus, IAsyncDisposable
{
    private readonly ILogger<RabbitMqReceiveMessageBus> _logger;
    private readonly RabbitMqOptions _options;
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;

    private IChannel? _eventChannel;
    private IChannel? _commandChannel;

    private string EventQueueName => $"{_options.ServiceName}.EventsInputQueue";
    private string CommandQueueName => $"{_options.ServiceName}.CommandsInputQueue";

    public RabbitMqReceiveMessageBus(
        IConnection connection,
        ILogger<RabbitMqReceiveMessageBus> logger,
        IOptions<RabbitMqOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _logger = logger;
        _options = options.Value;
        _scopeFactory = scopeFactory;
    }

    #region Initialization

    public Task InitializeAsync()
        => InitializeAsync(CancellationToken.None);

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _eventChannel = await _connection.CreateChannelAsync(new (true, true),cancellationToken);
        _commandChannel = await _connection.CreateChannelAsync(new (true, true),cancellationToken);

        // QoS (مهم برای جلوگیری از overload شدن سرویس)
        await _eventChannel.BasicQosAsync(0, 1, false, cancellationToken);
        await _commandChannel.BasicQosAsync(0, 1, false, cancellationToken);

        await DeclareInfrastructureAsync(_eventChannel, cancellationToken);
        await DeclareInfrastructureAsync(_commandChannel, cancellationToken);

        await CreateEventConsumerAsync(cancellationToken);
        await CreateCommandConsumerAsync(cancellationToken);
    }

    private async Task DeclareInfrastructureAsync(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Exchange {exchange} declared.", _options.ExchangeName);
    }

    #endregion

    #region Event Consumer

    private async Task CreateEventConsumerAsync(CancellationToken cancellationToken)
    {
        if (_eventChannel is null)
            throw new InvalidOperationException("Event channel not initialized.");

        await _eventChannel.QueueDeclareAsync(
            queue: EventQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_eventChannel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();

            try
            {
                StartActivity(ea);

                _logger.LogDebug("Event received with routing key {routingKey}", ea.RoutingKey);

                var messageConsumer = scope.ServiceProvider
                    .GetRequiredService<IMessageConsumer>();

                await messageConsumer.ConsumeEvent(
                    ea.BasicProperties?.AppId,
                    ea.ToParcel());

                await _eventChannel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing event.");
                await _eventChannel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _eventChannel.BasicConsumeAsync(
            queue: EventQueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Event queue {queue} ready.", EventQueueName);
    }

    #endregion

    #region Command Consumer

    private async Task CreateCommandConsumerAsync(CancellationToken cancellationToken)
    {
        if (_commandChannel is null)
            throw new InvalidOperationException("Command channel not initialized.");

        await _commandChannel.QueueDeclareAsync(
            queue: CommandQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_commandChannel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();

            try
            {
                StartActivity(ea);

                _logger.LogDebug("Command received with routing key {routingKey}", ea.RoutingKey);

                var messageConsumer = scope.ServiceProvider
                    .GetRequiredService<IMessageConsumer>();

                await messageConsumer.ConsumeCommand(
                    ea.BasicProperties?.AppId,
                    ea.ToParcel());

                await _commandChannel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing command.");
                await _commandChannel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _commandChannel.BasicConsumeAsync(
            queue: CommandQueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Command queue {queue} ready.", CommandQueueName);
    }

    #endregion

    #region Bindings

    public Task SubscribeAsync(string serviceId, string eventName)
        => SubscribeAsync(serviceId, eventName, CancellationToken.None);

    public async Task SubscribeAsync(
        string serviceId,
        string eventName,
        CancellationToken cancellationToken = default)
    {
        if (_eventChannel is null)
            throw new InvalidOperationException("Event channel not initialized.");

        var route = $"{serviceId}.event.{eventName}";

        await _eventChannel.QueueBindAsync(
            queue: EventQueueName,
            exchange: _options.ExchangeName,
            routingKey: route,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Bound event {route}", route);
    }

    public Task ReceiveAsync(string commandName)
        => ReceiveAsync(commandName, CancellationToken.None);

    public async Task ReceiveAsync(
        string commandName,
        CancellationToken cancellationToken = default)
    {
        if (_commandChannel is null)
            throw new InvalidOperationException("Command channel not initialized.");

        var route = $"{_options.ServiceName}.command.{commandName}";

        await _commandChannel.QueueBindAsync(
            queue: CommandQueueName,
            exchange: _options.ExchangeName,
            routingKey: route,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Bound command {route}", route);
    }

    #endregion

    #region Tracing

    private void StartActivity(BasicDeliverEventArgs ea)
    {
        var activity = new Activity("RabbitMqMessageReceived");
        activity.AddTag("ApplicationName", _options.ServiceName);

        if (ea.BasicProperties?.Headers is { } headers &&
            headers.TryGetValue("TraceId", out var traceObj) &&
            headers.TryGetValue("SpanId", out var spanObj) &&
            traceObj is ReadOnlyMemory<byte> traceBytes &&
            spanObj is ReadOnlyMemory<byte> spanBytes)
        {
            var traceId = Encoding.UTF8.GetString(traceBytes.Span);
            var spanId = Encoding.UTF8.GetString(spanBytes.Span);

            activity.SetParentId($"00-{traceId}-{spanId}-01");
        }

        activity.Start();
    }

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_eventChannel is not null)
                await _eventChannel.CloseAsync();

            if (_commandChannel is not null)
                await _commandChannel.CloseAsync();

            if (_connection.IsOpen)
                await _connection.CloseAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while disposing RabbitMqReceiveMessageBus.");
        }
    }

    #endregion
}