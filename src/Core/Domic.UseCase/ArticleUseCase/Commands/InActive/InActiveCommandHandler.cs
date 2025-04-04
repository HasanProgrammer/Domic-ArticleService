#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.ArticleUseCase.Commands.InActive;

public class InActiveCommandHandler(
    IDateTime dateTime, 
    ISerializer serializer, 
    IArticleCommandRepository articleCommandRepository,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<InActiveCommand, string>
{
    private readonly object _validationResult;

    public Task BeforeHandleAsync(InActiveCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(InActiveCommand command, CancellationToken cancellationToken)
    {
        var article = _validationResult as Article;
        
        article.InActive(dateTime, identityUser, serializer);

        await articleCommandRepository.ChangeAsync(article, cancellationToken);

        return article.Id;
    }

    public Task AfterHandleAsync(InActiveCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}