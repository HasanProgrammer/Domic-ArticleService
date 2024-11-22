using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

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

    public void BeforeHandle(UserInActived @event){}

    [TransactionConfig(Type = TransactionType.Command)]
    public void Handle(UserInActived @event)
    {
        var articles = _articleCommandRepository.FindByUserId(@event.Id);

        foreach (var article in articles)
        {
            article.InActive(_dotrisDateTime, @event.UpdatedBy, @event.UpdatedRole);
            
            _articleCommandRepository.Change(article);
        }
    }

    public void AfterHandle(UserInActived @event){}
}