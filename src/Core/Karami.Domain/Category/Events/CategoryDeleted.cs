using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Karami.Domain.Category.Events;

[MessageBroker(Queue = Broker.Article_Category_Queue)]
public class CategoryDeleted : UpdateDomainEvent<string>
{
    public string OwnerUsername { get; set; }
}