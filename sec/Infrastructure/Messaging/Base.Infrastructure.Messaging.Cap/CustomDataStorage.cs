using Base.Infrastructure.Messaging.Cap.Entity;

namespace Base.Infrastructure.Messaging.Cap;
internal class CustomDataStorage(
    PostgreSqlDataStorage dataStorage,
    IOptions<CapOptions> capOptions,
    ISerializer serializer)
    : IDataStorage
{
    internal static bool _disableRetriesForReceivedMessages;


    public Task<MediumMessage> StoreMessageAsync(string name, Message content, object? dbTransaction = null)
    {
        content.Headers.Add("hit-publish-machine-name", Environment.MachineName);

        // When UnitOfWork is not used 
        if (dbTransaction == null)
        {
            // save with ado.net on sql server
            return dataStorage.StoreMessageAsync(name, content);
        }
        // else :
        // add to DbContext and wait UnitOfWork.SaveChanges() to call

        if (dbTransaction is not DbContext dbContext)
        {
            throw new Exception("cap store message failed to resolve DbContext");
        }

        var message = new MediumMessage
        {
            DbId = content.GetId(),
            Origin = content,
            Content = serializer.Serialize(content),
            Added = DateTime.Now,
            ExpiresAt = null,
            Retries = 0
        };

        var entity = new Published
        {
            Id = long.Parse(message.DbId),
            Added = message.Added,
            Content = message.Content,
            ExpiresAt = message.ExpiresAt,
            Name = name,
            Retries = message.Retries,
            StatusName = nameof(StatusName.Scheduled),
            Version = capOptions.Value.Version,
        };

        dbContext.Add(entity);

        return Task.FromResult(message);
    }

    #region implemented by cap

    public Task ChangePublishStateAsync(MediumMessage message, StatusName state, object? transaction = null)
    {
        return dataStorage.ChangePublishStateAsync(message, state);
    }

    public Task ChangeReceiveStateAsync(MediumMessage message, StatusName state)
    {
        return dataStorage.ChangeReceiveStateAsync(message, state);
    }

    public Task<int> DeleteExpiresAsync(string table, DateTime timeout, int batchCount = 1000, CancellationToken token = default)
    {
        return dataStorage.DeleteExpiresAsync(table, timeout, batchCount, token);
    }

    public Task<int> DeletePublishedMessageAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IMonitoringApi GetMonitoringApi()
    {
        return dataStorage.GetMonitoringApi();
    }

    public Task StoreReceivedExceptionMessageAsync(string name, string group, string content)
    {
        return dataStorage.StoreReceivedExceptionMessageAsync(name, group, content);
    }

    public Task<MediumMessage> StoreReceivedMessageAsync(string name, string group, Message content)
    {
        return dataStorage.StoreReceivedMessageAsync(name, group, content);
    }

    public Task<bool> AcquireLockAsync(string key, TimeSpan ttl, string instance, CancellationToken token = default)
    {
        return dataStorage.AcquireLockAsync(key, ttl, instance, token);
    }

    public Task ReleaseLockAsync(string key, string instance, CancellationToken token = default)
    {
        return dataStorage.ReleaseLockAsync(key, instance, token);
    }

    public Task RenewLockAsync(string key, TimeSpan ttl, string instance, CancellationToken token = default)
    {
        return dataStorage.RenewLockAsync(key, ttl, instance, token);
    }

    public Task ChangePublishStateToDelayedAsync(string[] ids)
    {
        return dataStorage.ChangePublishStateToDelayedAsync(ids);
    }

    public Task ScheduleMessagesOfDelayedAsync(Func<object, IEnumerable<MediumMessage>, Task> scheduleTask, CancellationToken token = default)
    {
        return dataStorage.ScheduleMessagesOfDelayedAsync(scheduleTask, token);
    }

    public Task<IEnumerable<MediumMessage>> GetPublishedMessagesOfNeedRetry(TimeSpan lookbackSeconds)
    {
        return dataStorage.GetPublishedMessagesOfNeedRetry(lookbackSeconds);
    }

    public Task<IEnumerable<MediumMessage>> GetReceivedMessagesOfNeedRetry(TimeSpan lookbackSeconds)
    {
        if (_disableRetriesForReceivedMessages)
        {
            return Task.FromResult(Enumerable.Empty<MediumMessage>());
        }

        return dataStorage.GetReceivedMessagesOfNeedRetry(lookbackSeconds);
    }

    public Task<int> DeleteReceivedMessageAsync(long id)
    {
       return dataStorage.DeleteReceivedMessageAsync(id);
    }

    #endregion
}
