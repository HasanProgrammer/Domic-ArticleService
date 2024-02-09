using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Article.Entities;
using Karami.Domain.File.Contracts.Interfaces;

using File = Karami.Domain.File.Entities.File;

namespace Karami.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IFileCommandRepository    _fileCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;
    private readonly IGlobalUniqueIdGenerator  _idGenerator;

    public CreateCommandHandler(IFileCommandRepository fileCommandRepository, 
        IArticleCommandRepository articleCommandRepository, IDateTime dateTime, ISerializer serializer, 
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

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var fileId      = _idGenerator.GetRandom();
        var articleId   = _idGenerator.GetRandom();
        var createdBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var createdRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );

        var newArticle = new Article(
            _dateTime           ,
            articleId           ,
            createdBy           ,
            createdRole         ,
            command.CategoryId  ,
            command.Title       ,
            command.Summary     ,
            command.Body        ,
            fileId              ,
            command.FilePath    ,
            command.FileName    ,
            command.FileExtension
        );

        var newFile = new File(
            _dateTime        ,
            fileId           ,
            newArticle.Id    ,
            createdBy        ,
            createdRole      ,
            command.FilePath ,
            command.FileName ,
            command.FileName
        );

        await _articleCommandRepository.AddAsync(newArticle, cancellationToken);
        await _fileCommandRepository.AddAsync(newFile, cancellationToken);

        return newArticle.Id;
    }
}