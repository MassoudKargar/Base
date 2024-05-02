﻿namespace Base.Extensions.Events.PollingPublisher.Dal.Dapper.Options;

public class PollingPublisherDalRedisOptions
{
    public string ApplicationName { get; set; }
    public string ConnectionString { get; set; }
    public string SelectCommand { get; set; } = "Select top (@Count) * from Base.OutBoxEventItems where IsProcessed = 0";
    public string UpdateCommand { get; set; } = "Update Base.OutBoxEventItems set IsProcessed = 1 where OutBoxEventItemId in @Ids";
}