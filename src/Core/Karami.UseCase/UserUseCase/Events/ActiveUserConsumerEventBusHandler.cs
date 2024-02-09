using System.Data;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.User.Events;

namespace Karami.UseCase.UserUseCase.Events;

public class ActiveUserConsumerEventBusHandler : IConsumerEventBusHandler<UserActived>
{
    private readonly IDateTime                 _dateTime;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveUserConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository, IDateTime dateTime)
    {
        _dateTime                 = dateTime;
        _articleCommandRepository = articleCommandRepository;
    }

    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    public void Handle(UserActived @event)
    {
        var articles = _articleCommandRepository.FindByUserId(@event.Id);

        foreach (var article in articles)
        {
            article.Active(_dateTime, @event.UpdatedBy, @event.UpdatedRole);
            
            _articleCommandRepository.Change(article);
        }
    }
}