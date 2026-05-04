using Base.Core.Domains.Contracts.Events;

namespace Base.Core.Domains.Contracts.Common;

public interface IIntegrationEventDetector
{
    List<IIntegrationEvent> GetIntegrationEvents();
}