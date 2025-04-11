#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Domic.Domain.File.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

using File = Domic.Domain.File.Entities.File;

namespace Domic.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommandHandler(
    IDateTime dateTime,
    ISerializer serializer,
    IArticleCommandRepository articleCommandRepository,
    IFileCommandRepository fileCommandRepository,
    IGlobalUniqueIdGenerator idGenerator,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetArticle = _validationResult as Article;

        var newFileId = idGenerator.GetRandom();

        targetArticle.Change(
            dateTime           ,
            identityUser       ,
            serializer         ,
            command.CategoryId ,
            command.Title      ,
            command.Summary    ,
            command.Body       ,
            newFileId          ,
            command.FilePath   ,
            command.FileName   ,
            command.FileExtension
        );

        if (!string.IsNullOrEmpty(command.FileName))
        {
            var targetFile =
                await fileCommandRepository.FindByArticleIdAsync(targetArticle.Id, cancellationToken);

            await fileCommandRepository.RemoveAsync(targetFile, cancellationToken);

            var newFile =
                new File(dateTime, idGenerator, serializer, identityUser, targetArticle.Id, command.FilePath, 
                    command.FileName, command.FileExtension
                );

            await fileCommandRepository.AddAsync(newFile, cancellationToken);
        }

        await articleCommandRepository.ChangeAsync(targetArticle, cancellationToken);

        return targetArticle.Id;
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}