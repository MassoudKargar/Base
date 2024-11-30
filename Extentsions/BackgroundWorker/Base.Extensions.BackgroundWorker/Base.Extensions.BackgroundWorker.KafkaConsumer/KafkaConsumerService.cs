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
    private IConsumer<TKey, TValue> consumer;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _configurationKafka.GetConfiguration();
        consumer = new ConsumerBuilder<TKey, TValue>(_configurationKafka.ConsumerConfig).Build();
        consumer.Subscribe(_configurationKafka.InputTopic);
        await base.StartAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            using (new Activity(nameof(KafkaConsumerService<TKey, TValue>)).Start())
            {
                consumer.Subscribe(new List<string>() { _configurationKafka.InputTopic });
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Kafka Consumer Service has started.");
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult?.Message == null) continue;
                        if (consumeResult.Topic.Equals(_configurationKafka.InputTopic))
                        {
                            await Task.Run(async () =>
                            {
                                await Consume(consumeResult, stoppingToken);
                            }, stoppingToken).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
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
        consumer.Close();
        return base.StopAsync(cancellationToken);
    }


    public override void Dispose()
    {
        consumer.Dispose();
        base.Dispose();
    }

}
