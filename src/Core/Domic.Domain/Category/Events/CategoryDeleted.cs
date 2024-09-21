using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Category.Events;

[EventConfig(Queue = Broker.Article_Category_Queue)]
public class CategoryDeleted : UpdateDomainEvent<string>
{
    public string OwnerUsername { get; set; }
}