using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Category.Events;
using Domic.Domain.File.Contracts.Interfaces;

namespace Domic.UseCase.CategoryUseCase.Events;

public class DeleteCategoryConsumerEventBusHandler(
    IDateTime dateTime, 
    IFileCommandRepository fileCommandRepository, 
    IArticleCommandRepository articleCommandRepository
) : IConsumerEventBusHandler<CategoryDeleted>
{
    public Task BeforeHandleAsync(CategoryDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Articles)]
    [TransactionConfig(Type = TransactionType.Command)]
    public async Task HandleAsync(CategoryDeleted @event, CancellationToken cancellationToken)
    {
        var targetArticles =
            await articleCommandRepository.FindByCategoryIdEagerLoadingAsync(@event.Id, cancellationToken);

        foreach (var article in targetArticles)
        {
            article.Delete(dateTime, @event.UpdatedBy, @event.UpdatedRole);
           
            await articleCommandRepository.ChangeAsync(article, cancellationToken);

            #region HardDelete Files

            await fileCommandRepository.RemoveRangeAsync(article.Files, cancellationToken);

            #endregion
        }
    }

    public Task AfterHandleAsync(CategoryDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}