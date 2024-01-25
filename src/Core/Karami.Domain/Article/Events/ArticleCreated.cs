using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.Article.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.Article_Article_Exchange)]
public class ArticleCreated : CreateDomainEvent<string>
{
    //Article info ( Aggregte entity | Aggregte root )
    
    public string CategoryId { get; init; }
    public string Title      { get; init; }
    public string Summary    { get; init; }
    public string Body       { get; init; }
    
    //File info
    
    public string FileId        { get; init; }
    public string FilePath      { get; init; }
    public string FileName      { get; init; }
    public string FileExtension { get; init; }
}