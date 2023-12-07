#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Article.Entities;
using Karami.Domain.File.Contracts.Interfaces;

using Action = Karami.Core.Common.ClassConsts.Action;
using File   = Karami.Domain.File.Entities.File;

namespace Karami.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IDotrisDateTime           _dotrisDateTime;
    private readonly ISerializer               _serializer;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IEventCommandRepository   _eventCommandRepository;

    public UpdateCommandHandler(IArticleCommandRepository articleCommandRepository,
        IFileCommandRepository fileCommandRepository, 
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime, 
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime           = dotrisDateTime;
        _serializer               = serializer;
        _jsonWebToken             = jsonWebToken;
        _fileCommandRepository    = fileCommandRepository;
        _articleCommandRepository = articleCommandRepository;
        _eventCommandRepository   = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetArticle = _validationResult as Article;
        
        var newFileId = Guid.NewGuid().ToString();

        targetArticle.Change(
            _dotrisDateTime    ,
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
                await _fileCommandRepository.FindByArticleIdAsync(targetArticle.Id, cancellationToken);

            _fileCommandRepository.Remove(targetFile);

            var newFile =
                new File(_dotrisDateTime, newFileId, targetArticle.Id, command.FilePath, command.FileName, command.FileExtension);

            await _fileCommandRepository.AddAsync(newFile, cancellationToken);
        }

        _articleCommandRepository.Change(targetArticle);

        #region OutBox

        var events = targetArticle.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer,
            Service.ArticleService, Table.ArticleTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (var @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);

        #endregion

        return targetArticle.Id;
    }
}