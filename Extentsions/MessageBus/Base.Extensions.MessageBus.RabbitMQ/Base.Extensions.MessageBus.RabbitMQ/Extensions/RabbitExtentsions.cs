namespace Base.Extensions.MessageBus.RabbitMQ.Extensions;
static class RabbitExtensions
{
    public static Parcel ToParcel(this BasicDeliverEventArgs basicDeliverEventArgs)
    {
        Parcel parcel = new(messageName: basicDeliverEventArgs.BasicProperties.Type,
            body: Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray()),
            route: basicDeliverEventArgs.RoutingKey,
            messageId: basicDeliverEventArgs.BasicProperties.MessageId,
                correlationId: basicDeliverEventArgs.BasicProperties.CorrelationId,
                headers: basicDeliverEventArgs?.BasicProperties.Headers != null 
                    ? (Dictionary<string, string>)basicDeliverEventArgs?.BasicProperties?.Headers : null
                );
        return parcel;
    }
}
