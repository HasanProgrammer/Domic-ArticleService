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
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IEventCommandRepository   _eventCommandRepository;
    private readonly IGlobalUniqueIdGenerator  _idGenerator;

    public UpdateCommandHandler(IArticleCommandRepository articleCommandRepository, 
        IFileCommandRepository fileCommandRepository, IEventCommandRepository eventCommandRepository, 
        IDateTime dateTime, ISerializer serializer, IJsonWebToken jsonWebToken, 
        IGlobalUniqueIdGenerator idGenerator
    )
    {
        _dateTime                 = dateTime;
        _serializer               = serializer;
        _jsonWebToken             = jsonWebToken;
        _fileCommandRepository    = fileCommandRepository;
        _articleCommandRepository = articleCommandRepository;
        _eventCommandRepository   = eventCommandRepository;
        _idGenerator              = idGenerator;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetArticle = _validationResult as Article;

        var newFileId = _idGenerator.GetRandom();

        var identityUserId = _jsonWebToken.GetIdentityUserId(command.Token);

        targetArticle.Change(
            _dateTime          ,
            command.CategoryId ,
            identityUserId     ,
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
                new File(_dateTime, newFileId, targetArticle.Id, identityUserId, command.FilePath, 
                    command.FileName, command.FileExtension
                );

            //ToDo : ( Tech Debt ) -> Should be used [Add] insted [AddAsync]
            await _fileCommandRepository.AddAsync(newFile, cancellationToken);
        }

        _articleCommandRepository.Change(targetArticle);

        #region OutBox
        
        var events = targetArticle.GetEvents.ToEntityOfEvent(_dateTime, _serializer,
            Service.ArticleService, Table.ArticleTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (var @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);

        #endregion

        return targetArticle.Id;
    }
}