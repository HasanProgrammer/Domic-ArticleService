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

namespace Karami.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDotrisDateTime           _dotrisDateTime;
    private readonly ISerializer               _serializer;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IEventCommandRepository   _eventCommandRepository;

    public CreateCommandHandler(IFileCommandRepository fileCommandRepository, 
        IArticleCommandRepository articleCommandRepository, 
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
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var fileId = Guid.NewGuid().ToString();
        
        var newArticle = new Article(
            _dotrisDateTime           ,
            Guid.NewGuid().ToString() ,
            command.UserId            ,
            command.CategoryId        ,
            command.Title             ,
            command.Summary           ,
            command.Body              ,
            fileId                    ,
            command.FilePath          ,
            command.FileName          ,
            command.FileExtension
        );

        var newFile = new File(
            _dotrisDateTime  ,
            fileId           ,
            newArticle.Id    ,
            command.FilePath ,
            command.FileName ,
            command.FileName
        );

        await _articleCommandRepository.AddAsync(newArticle, cancellationToken);
        await _fileCommandRepository.AddAsync(newFile, cancellationToken);

        #region OutBox

        var events = newArticle.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.ArticleService, 
            Table.ArticleTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (var @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);

        #endregion

        return newArticle.Id;
    }
}