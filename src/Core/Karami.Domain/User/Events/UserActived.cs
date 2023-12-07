using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Karami.Domain.User.Events;

[MessageBroker(Queue = Broker.Article_User_Queue)]
public class UserActived : UpdateDomainEvent
{
    public required string Id            { get; init; }
    public required string OwnerUsername { get; init; }
}