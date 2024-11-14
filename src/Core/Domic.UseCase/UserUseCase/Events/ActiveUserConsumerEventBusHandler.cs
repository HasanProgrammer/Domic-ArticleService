using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class ActiveUserConsumerEventBusHandler : IConsumerEventBusHandler<UserActived>
{
    private readonly IDateTime                 _dateTime;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveUserConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository, IDateTime dateTime)
    {
        _dateTime                 = dateTime;
        _articleCommandRepository = articleCommandRepository;
    }

    [TransactionConfig(Type = TransactionType.Command)]
    public void Handle(UserActived @event)
    {
        var articles = _articleCommandRepository.FindByUserId(@event.Id);

        foreach (var article in articles)
        {
            article.Active(_dateTime, @event.UpdatedBy, @event.UpdatedRole);
            
            _articleCommandRepository.Change(article);
        }
    }

    public void AfterTransactionHandle(UserActived @event){}
}