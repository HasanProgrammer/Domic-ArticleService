#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Article.Entities;

namespace Karami.UseCase.ArticleUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object _validationResult;
    
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public DeleteCommandHandler(IArticleCommandRepository articleCommandRepository, IDateTime dateTime, 
        ISerializer serializer, IJsonWebToken jsonWebToken
    )
    {
        _dateTime                 = dateTime;
        _serializer               = serializer;
        _jsonWebToken             = jsonWebToken;
        _articleCommandRepository = articleCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var article     = _validationResult as Article;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        article.Delete(_dateTime, updatedBy, updatedRole);

        _articleCommandRepository.Change(article);

        return Task.FromResult(article.Id);
    }
}