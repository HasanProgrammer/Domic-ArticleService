using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class InActiveUserConsumerEventBusHandler(IArticleCommandRepository articleCommandRepository, IDateTime dateTime) 
    : IConsumerEventBusHandler<UserInActived>
{
    public Task BeforeHandleAsync(UserInActived @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithCleanCache(Keies = Cache.Articles)]
    [TransactionConfig(Type = TransactionType.Command)]
    public async Task HandleAsync(UserInActived @event, CancellationToken cancellationToken)
    {
        var articles = await articleCommandRepository.FindByUserIdAsync(@event.Id, cancellationToken);

        foreach (var article in articles)
        {
            article.InActive(dateTime, @event.UpdatedBy, @event.UpdatedRole);
            
            await articleCommandRepository.ChangeAsync(article, cancellationToken);
        }
    }

    public Task AfterHandleAsync(UserInActived @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}