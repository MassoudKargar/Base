using Base.Extensions.BackgroundWorker.Abstractions;
using Base.Extensions.BackgroundWorker.KafkaConsumer;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Base.Extensions.BackgroundWorker.KafkaProducer;

public abstract class KafkaProducerService<TKey, TValue>(IKafkaProducerConfiguration configurationKafka, ILogger<KafkaProducerService<TKey, TValue>> logger)
    : AbstractBackgroundWorker
{
    private readonly IKafkaProducerConfiguration _configurationKafka = configurationKafka;
    private readonly ILogger<KafkaProducerService<TKey, TValue>> _logger = logger;
    private IProducer<TKey, TValue> Producer;
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _configurationKafka.GetConfiguration();
        Producer = new ProducerBuilder<TKey, TValue>(_configurationKafka.ProducerConfig).Build();
        await base.StartAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Kafka Producer Service has started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
    public virtual async Task Produce(TKey key, TValue value, CancellationToken cancellationToken)
    {
        try
        {
            using (new Activity(nameof(KafkaProducerService<TKey, TValue>)).Start())
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var msg = new Message<TKey, TValue>
                    {
                        Key = key,
                        Value = value
                    };
                    await Producer.ProduceAsync(_configurationKafka.OutputTopic, msg, cancellationToken);

                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    public override void Dispose()
    {
        Producer.Dispose();
        base.Dispose();
    }
}
