using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[EventConfig(Queue = Broker.Article_User_Queue)]
public class UserInActived : UpdateDomainEvent<string>
{
    public required string OwnerUsername { get; init; }
}