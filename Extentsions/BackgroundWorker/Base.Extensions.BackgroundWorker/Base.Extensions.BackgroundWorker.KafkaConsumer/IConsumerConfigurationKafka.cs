using Confluent.Kafka;

namespace Base.Extensions.BackgroundWorker.KafkaConsumer;

public interface IConsumerConfigurationKafka
{
    public string InputTopic { get; set; }
    public string OutputTopic { get; set; }
    public ConsumerConfig ConsumerConfig { get; set; }
    public Task GetConfiguration();
}
