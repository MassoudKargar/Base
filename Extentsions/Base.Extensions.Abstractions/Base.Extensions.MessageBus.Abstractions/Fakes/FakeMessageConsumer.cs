
namespace Base.Extensions.MessageBus.Abstractions.Fakes;

public sealed class FakeMessageConsumer : IMessageConsumer
{
    public List<(string Sender, Parcel Parcel)> ConsumedCommands { get; } = new();
    public List<(string Sender, Parcel Parcel)> ConsumedEvents { get; } = new();

    public bool ShouldFail { get; set; }

    public Task<bool> ConsumeCommand(string sender, Parcel parcel)
    {
        if (ShouldFail)
            throw new InvalidOperationException("Fake command failure");

        ConsumedCommands.Add((sender, parcel));

        return Task.FromResult(true);
    }

    public Task<bool> ConsumeEvent(string sender, Parcel parcel)
    {
        if (ShouldFail)
            throw new InvalidOperationException("Fake event failure");

        ConsumedEvents.Add((sender, parcel));

        return Task.FromResult(true);
    }
}