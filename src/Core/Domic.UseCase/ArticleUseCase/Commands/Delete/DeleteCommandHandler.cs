#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.ArticleUseCase.Commands.Delete;

public class DeleteCommandHandler(
    IDateTime dateTime, 
    ISerializer serializer,
    IArticleCommandRepository articleCommandRepository,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<DeleteCommand, string>
{
    private readonly object _validationResult;

    public Task BeforeHandleAsync(DeleteCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var article = _validationResult as Article;
        
        article.Delete(dateTime, identityUser, serializer);

        await articleCommandRepository.ChangeAsync(article, cancellationToken);

        return article.Id;
    }

    public Task AfterHandleAsync(DeleteCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}