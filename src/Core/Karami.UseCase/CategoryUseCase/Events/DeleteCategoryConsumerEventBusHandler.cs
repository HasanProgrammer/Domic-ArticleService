using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Category.Events;
using Karami.Domain.File.Contracts.Interfaces;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.CategoryUseCase.Events;

public class DeleteCategoryConsumerEventBusHandler : IConsumerEventBusHandler<CategoryDeleted>
{
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IEventCommandRepository   _eventCommandRepository;

    public DeleteCategoryConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository,
        IFileCommandRepository fileCommandRepository, IEventCommandRepository eventCommandRepository,
        IDateTime dateTime, ISerializer serializer
    )
    {
        _dateTime                 = dateTime;
        _serializer               = serializer;
        _fileCommandRepository    = fileCommandRepository;
        _articleCommandRepository = articleCommandRepository;
        _eventCommandRepository   = eventCommandRepository;
    }
    
    [WithTransaction]
    [WithCleanCache(Keies = Cache.Articles)]
    public void Handle(CategoryDeleted @event)
    {
        var targetArticles =
            _articleCommandRepository.FindByCategoryIdEagerLoadingAsync(@event.Id, default).GetAwaiter().GetResult();

        foreach (var article in targetArticles)
        {
            article.Delete(_dateTime, @event.UpdatedBy);
           
            _articleCommandRepository.Change(article);

            #region OutBox

            var newEvents = article.GetEvents.ToEntityOfEvent(_dateTime, _serializer, Service.ArticleService,
                Table.ArticleTable, Action.Delete, @event.OwnerUsername
            );

            foreach (var newEvent in newEvents)
                _eventCommandRepository.Add(newEvent);

            #endregion

            #region HardDelete Files

            _fileCommandRepository.RemoveRange(article.Files);

            #endregion
        }
    }
}