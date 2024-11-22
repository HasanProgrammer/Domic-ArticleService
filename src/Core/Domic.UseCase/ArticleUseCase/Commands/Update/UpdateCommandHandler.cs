#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Domic.Domain.File.Contracts.Interfaces;

using File = Domic.Domain.File.Entities.File;

namespace Domic.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IGlobalUniqueIdGenerator  _idGenerator;

    public UpdateCommandHandler(IArticleCommandRepository articleCommandRepository, 
        IFileCommandRepository fileCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken, IGlobalUniqueIdGenerator idGenerator
    )
    {
        _dateTime                 = dateTime;
        _serializer               = serializer;
        _jsonWebToken             = jsonWebToken;
        _fileCommandRepository    = fileCommandRepository;
        _articleCommandRepository = articleCommandRepository;
        _idGenerator              = idGenerator;
    }

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetArticle = _validationResult as Article;

        var newFileId = _idGenerator.GetRandom();

        var updatedBy = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );

        targetArticle.Change(
            _dateTime          ,
            command.CategoryId ,
            updatedBy          ,
            updatedRole        ,
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
                await _fileCommandRepository.FindByArticleIdAsync(targetArticle.Id, cancellationToken);

            _fileCommandRepository.Remove(targetFile);

            var newFile =
                new File(_dateTime, newFileId, targetArticle.Id, updatedBy, updatedRole, command.FilePath, 
                    command.FileName, command.FileExtension
                );

            //ToDo : ( Tech Debt ) -> Should be used [Add] insted [AddAsync]
            await _fileCommandRepository.AddAsync(newFile, cancellationToken);
        }

        _articleCommandRepository.Change(targetArticle);

        return targetArticle.Id;
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}