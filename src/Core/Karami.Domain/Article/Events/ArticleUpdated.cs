using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.Article.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.Article_Article_Exchange)]
public class ArticleUpdated : UpdateDomainEvent
{
    //Article info ( Aggregte entity | Aggregte root )
    
    public required string Id         { get; init; }
    public required string CategoryId { get; init; }
    public required string Title      { get; init; }
    public required string Summary    { get; init; }
    public required string Body       { get; init; }
    
    //File info
    
    public required string FileId        { get; init; }
    public required string FilePath      { get; init; }
    public required string FileName      { get; init; }
    public required string FileExtension { get; init; }
}