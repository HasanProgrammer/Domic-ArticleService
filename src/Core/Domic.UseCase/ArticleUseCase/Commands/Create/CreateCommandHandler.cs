using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Domic.Domain.File.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

using File = Domic.Domain.File.Entities.File;

namespace Domic.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommandHandler(
    IFileCommandRepository fileCommandRepository, 
    IArticleCommandRepository articleCommandRepository, 
    IDateTime dateTime, 
    ISerializer serializer,
    IGlobalUniqueIdGenerator idGenerator,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<CreateCommand, string>
{
    public Task BeforeHandleAsync(CreateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    [WithCleanCache(Keies = Cache.Articles)]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var fileId = idGenerator.GetRandom();

        var newArticle = new Article(
            dateTime,
            idGenerator,
            serializer,
            identityUser,
            command.CategoryId,
            command.Title       ,
            command.Summary     ,
            command.Body        ,
            fileId              ,
            command.FilePath    ,
            command.FileName    ,
            command.FileExtension
        );

        var newFile = new File(
            dateTime,
            idGenerator,
            serializer,
            identityUser,
            newArticle.Id,
            command.FilePath,
            command.FileName,
            command.FileExtension
        );

        await articleCommandRepository.AddAsync(newArticle, cancellationToken);
        await fileCommandRepository.AddAsync(newFile, cancellationToken);

        return newArticle.Id;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}