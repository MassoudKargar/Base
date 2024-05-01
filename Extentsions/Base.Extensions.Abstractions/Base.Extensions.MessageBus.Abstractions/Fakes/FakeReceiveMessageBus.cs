using Microsoft.Extensions.Logging;

namespace Base.Extensions.MessageBus.Abstractions.Fakes;

public class FakeReceiveMessageBus(ILogger<FakeSendMessageBus> logger) : IReceiveMessageBus
{
    public void Receive(string commandName)
    {
        logger.LogInformation("fake message bus receive {commandName}", commandName);
    }

    public void Subscribe(string serviceId, string eventName)
    {
        logger.LogInformation("fake message bus subscribe for event: {eventName} from service {serviceId}", eventName, serviceId);
    }
}
