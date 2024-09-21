using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.Article.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = Broker.Article_Article_Exchange)]
public class ArticleUpdated : UpdateDomainEvent<string>
{
    //Article info ( Aggregte entity | Aggregte root )
    
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