#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommandHandler(
    IArticleCommandRepository articleCommandRepository,
    IDateTime dateTime,
    ISerializer serializer,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;

    public Task BeforeHandleAsync(ActiveCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var article = _validationResult as Article;
        
        article.Active(dateTime, identityUser, serializer);

        await articleCommandRepository.ChangeAsync(article, cancellationToken);

        return article.Id;
    }

    public Task AfterHandleAsync(ActiveCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}