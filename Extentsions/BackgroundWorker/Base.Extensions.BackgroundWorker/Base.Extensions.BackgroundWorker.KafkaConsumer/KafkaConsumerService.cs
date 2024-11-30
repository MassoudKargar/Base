using Base.Extensions.BackgroundWorker.Abstractions;

using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Base.Extensions.BackgroundWorker.KafkaConsumer;

public interface IConsumerConfigurationKafka
{
    public string InputTopic { get; set; }
    public string OutputTopic { get; set; }
    public ConsumerConfig ConsumerConfig { get; set; }

    public Task GetConfiguration();
}


public class KafkaConsumerService<TKey,TValue> : AbstractBackgroundWorker, IKafkaConsumerService
{
    private readonly IConsumerConfigurationKafka _configurationKafka;
    private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
    private readonly IConsumer<TKey, TValue> _consumer;
    public KafkaConsumerService(IConsumerConfigurationKafka configurationKafka, ILogger<KafkaConsumerService<TKey, TValue>> logger)
    {
        _configurationKafka = configurationKafka;
        _logger = logger;
        _consumer = new ConsumerBuilder<TKey, TValue>(_configurationKafka.ConsumerConfig).Build();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka Consumer Start : ");
        _consumer.Subscribe(_configurationKafka.InputTopic);
        return base.StartAsync(cancellationToken);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }


    public override void Dispose()
    {
        base.Dispose();
    }

}

public interface IKafkaConsumerService
{

}