using Confluent.Kafka;

namespace Base.Extensions.BackgroundWorker.KafkaConsumer;

public interface IKafkaProducerConfiguration
{
    public string OutputTopic { get; set; }
    public ProducerConfig ProducerConfig { get; set; }
    public Task GetConfiguration();
}
