using System.Data;
using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.User.Events;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.UserUseCase.Events;

public class ActiveUserConsumerEventBusHandler : IConsumerEventBusHandler<UserActived>
{
    private readonly IDotrisDateTime           _dotrisDateTime;
    private readonly ISerializer               _serializer;
    private readonly IEventCommandRepository   _eventCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveUserConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository,
        IEventCommandRepository eventCommandRepository, IDotrisDateTime dotrisDateTime, ISerializer serializer
    )
    {
        _dotrisDateTime           = dotrisDateTime;
        _serializer               = serializer;
        _eventCommandRepository   = eventCommandRepository;
        _articleCommandRepository = articleCommandRepository;
    }

    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    public void Handle(UserActived @event)
    {
        var articles = _articleCommandRepository.FindByUserId(@event.Id);

        foreach (var article in articles)
        {
            article.Active(_dotrisDateTime);
            
            _articleCommandRepository.Change(article);
            
            #region OutBox

            var newEvents = article.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.ArticleService,
                Table.ArticleTable, Action.Update, @event.OwnerUsername
            );

            foreach (var newEvent in newEvents)
                _eventCommandRepository.Add(newEvent);

            #endregion
        }
    }
}