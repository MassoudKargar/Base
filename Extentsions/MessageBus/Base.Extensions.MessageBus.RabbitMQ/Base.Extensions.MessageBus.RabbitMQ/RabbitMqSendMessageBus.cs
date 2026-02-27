namespace Base.Extensions.MessageBus.RabbitMQ;

public sealed class RabbitMqSendMessageBus(
    IConnection connection,
    IJsonSerializer jsonSerializer,
    IOptions<RabbitMqOptions> rabbitMqOptions,
    ILogger<RabbitMqSendMessageBus> logger)
    : IAsyncDisposable, ISendMessageBus
{
    private IChannel? _channel;
    private readonly RabbitMqOptions _options = rabbitMqOptions.Value;

    /// <summary>
    /// Async initialization: creates channel and exchange
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_channel != null) return;

        _channel = await connection.CreateChannelAsync(new CreateChannelOptions(true, true), cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Exchange {exchange} declared.", _options.ExchangeName);
    }


    public async Task PublishAsync<TEvent>(TEvent @event, MessageContext? context = null,
        CancellationToken cancellationToken = new()) where TEvent : class
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));

        var messageName = @event.GetType().Name;

        var parcel = new Parcel(
            messageName: messageName,
            body: jsonSerializer.Serialize(@event),
            route: $"{_options.ServiceName}.{RabbitMqSendMessageBusConstants.@event}.{messageName}",
            messageId: Guid.NewGuid().ToString(),
            correlationId: Guid.NewGuid().ToString(),
            headers: new Dictionary<string, string>
            {
                ["AccuredOn"] = DateTime.UtcNow.ToString("o")
            });

        await SendAsync(parcel, cancellationToken);
    }

    public async Task SendCommandAsync<TCommand>(string destinationService, TCommand command, MessageContext? context = null,
        CancellationToken cancellationToken = new CancellationToken()) where TCommand : class
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        var commandName = command.GetType().Name;

        var parcel = new Parcel(
            messageName: commandName,
            body: jsonSerializer.Serialize(command),
            route: $"{_options.ServiceName}.{RabbitMqSendMessageBusConstants.@event}.{commandName}",
            messageId: Guid.NewGuid().ToString(),
            correlationId: Guid.NewGuid().ToString(),
            headers: new Dictionary<string, string>
            {
                ["AccuredOn"] = DateTime.UtcNow.ToString("o")
            });
        await SendAsync(parcel, cancellationToken);
    }

    public async Task SendAsync(Parcel parcel, CancellationToken cancellationToken = default)
    {
        if (parcel == null) throw new ArgumentNullException(nameof(parcel));
        if (_channel == null) throw new InvalidOperationException("Channel is not initialized. Call InitializeAsync first.");

        //var activity = StartChildActivity(parcel);
        //AddActivityHeaders(parcel, activity);


        var body = Encoding.UTF8.GetBytes(parcel.Body);

        await _channel.BasicPublishAsync(_options.ExchangeName, parcel.Route,true, body, cancellationToken: cancellationToken);

        logger.LogDebug("Message Sent {MessageName} to route {Route}", parcel.MessageName, parcel.Route);
    }

    //private static void AddActivityHeaders(Parcel parcel, Activity activity)
    //{
    //    if (parcel.Headers == null) parcel.Headers = new Dictionary<string, string>();
    //    parcel.Headers["TraceId"] = activity.TraceId.ToHexString();
    //    parcel.Headers["SpanId"] = activity.SpanId.ToHexString();
    //}

    private Activity StartChildActivity(Parcel parcel)
    {
        var activity = new Activity("SendParcel");
        activity.AddTag("ParcelName", parcel.MessageName);
        activity.AddTag("ApplicationName", _options.ServiceName);
        activity.Start();
        return activity;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            try
            {
                if (_channel.IsOpen)
                    await _channel.CloseAsync();
                _channel.Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error closing RabbitMQ channel");
            }
        }
    }
}