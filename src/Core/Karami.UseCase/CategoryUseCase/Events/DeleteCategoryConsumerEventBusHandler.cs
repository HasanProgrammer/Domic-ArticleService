using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Category.Events;
using Karami.Domain.File.Contracts.Interfaces;

namespace Karami.UseCase.CategoryUseCase.Events;

public class DeleteCategoryConsumerEventBusHandler : IConsumerEventBusHandler<CategoryDeleted>
{
    private readonly IDateTime                 _dateTime;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public DeleteCategoryConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository,
        IFileCommandRepository fileCommandRepository, IDateTime dateTime
    )
    {
        _dateTime                 = dateTime;
        _fileCommandRepository    = fileCommandRepository;
        _articleCommandRepository = articleCommandRepository;
    }
    
    [WithTransaction]
    [WithCleanCache(Keies = Cache.Articles)]
    public void Handle(CategoryDeleted @event)
    {
        var targetArticles =
            _articleCommandRepository.FindByCategoryIdEagerLoadingAsync(@event.Id, default).GetAwaiter().GetResult();

        foreach (var article in targetArticles)
        {
            article.Delete(_dateTime, @event.UpdatedBy, @event.UpdatedRole);
           
            _articleCommandRepository.Change(article);

            #region HardDelete Files

            _fileCommandRepository.RemoveRange(article.Files);

            #endregion
        }
    }
}