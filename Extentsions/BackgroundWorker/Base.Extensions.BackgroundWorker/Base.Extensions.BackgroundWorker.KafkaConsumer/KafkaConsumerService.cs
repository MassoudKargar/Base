using Base.Extensions.BackgroundWorker.Abstractions;

using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Base.Extensions.BackgroundWorker.KafkaConsumer;


public abstract class KafkaConsumerService<TKey, TValue>
    (IKafkaConsumerConfiguration configurationKafka, ILogger<KafkaConsumerService<TKey, TValue>> logger)
    : AbstractBackgroundWorker
{
    private readonly IKafkaConsumerConfiguration _configurationKafka = configurationKafka;
    private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger = logger;
    private IConsumer<TKey, TValue> Consumer;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _configurationKafka.GetConfiguration();
        Consumer = new ConsumerBuilder<TKey, TValue>(_configurationKafka.ConsumerConfig).Build();
        Consumer.Subscribe(_configurationKafka.InputTopic);
        await base.StartAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            Consumer.Subscribe(new List<string>() { _configurationKafka.InputTopic });
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (new Activity(nameof(KafkaConsumerService<TKey, TValue>)).Start())
                    {
                        _logger.LogInformation("Kafka Consumer Service has started.");
                        var consumeResult = Consumer.Consume(stoppingToken);
                        if (consumeResult?.Message == null) continue;
                        if (consumeResult.Topic.Equals(_configurationKafka.InputTopic))
                        {
                            await Task.Run(async () =>
                            {
                                await Consume(consumeResult, stoppingToken);
                            }, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }
        finally
        {
            sw.Stop();
        }
        throw new NotImplementedException();
    }
    protected abstract Task Consume(ConsumeResult<TKey, TValue> consumeResult, CancellationToken cancellationToken);
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Consumer.Close();
        return base.StopAsync(cancellationToken);
    }


    public override void Dispose()
    {
        Consumer.Dispose();
        base.Dispose();
    }

}
