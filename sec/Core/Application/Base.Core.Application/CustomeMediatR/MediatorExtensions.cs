namespace Base.Core.Application.CustomeMediatR;
public static class MediatorExtensions
{
    public static Task Publish<TNotification>(this IMediator mediator, TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        if (mediator is CustomMediator customMediator)
        {
            return customMediator.Publish(notification, strategy, cancellationToken);
        }

        throw new NotImplementedException("The custom mediator implementation is not registered!");
    }
}