using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.Article.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.Article_Article_Exchange)]
public class ArticleDeleted : UpdateDomainEvent<string>
{
    
}