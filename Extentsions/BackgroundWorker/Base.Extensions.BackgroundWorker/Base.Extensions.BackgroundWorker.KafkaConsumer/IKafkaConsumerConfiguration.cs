using Confluent.Kafka;

namespace Base.Extensions.BackgroundWorker.KafkaConsumer;

public interface IKafkaConsumerConfiguration
{
    public string InputTopic { get; set; }
    public ConsumerConfig ConsumerConfig { get; set; }
    public Task GetConfiguration();
}
