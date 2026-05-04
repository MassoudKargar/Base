namespace Base.Infrastructure.Messaging.Cap;
internal static class Common
{
    public static string? Schema = null;
    public const string PublishedTableName = "MessagingOutbox";
    public const string ReceivedTableName = "MessagingInbox";
    public const string LockTableName = "MessagingLock";
}