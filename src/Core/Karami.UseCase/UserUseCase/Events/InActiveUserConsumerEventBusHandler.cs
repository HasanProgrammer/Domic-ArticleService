using System.Data;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.User.Events;

namespace Karami.UseCase.UserUseCase.Events;

public class InActiveUserConsumerEventBusHandler : IConsumerEventBusHandler<UserInActived>
{
    private readonly IDateTime                 _dotrisDateTime;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public InActiveUserConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository, 
        IDateTime dotrisDateTime
    )
    {
        _dotrisDateTime           = dotrisDateTime;
        _articleCommandRepository = articleCommandRepository;
    }
    
    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    public void Handle(UserInActived @event)
    {
        var articles = _articleCommandRepository.FindByUserId(@event.Id);

        foreach (var article in articles)
        {
            article.InActive(_dotrisDateTime, @event.UpdatedBy, @event.UpdatedRole);
            
            _articleCommandRepository.Change(article);
        }
    }
}