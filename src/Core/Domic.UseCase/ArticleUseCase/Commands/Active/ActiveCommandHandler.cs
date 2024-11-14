﻿#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;

namespace Domic.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommandHandler : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;
    
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveCommandHandler(IArticleCommandRepository articleCommandRepository, IDateTime dateTime, 
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
    public Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var article     = _validationResult as Article;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        article.Active(_dateTime, updatedBy, updatedRole);

        _articleCommandRepository.Change(article);

        return Task.FromResult(article.Id);
    }

    public Task AfterTransactionHandleAsync(ActiveCommand message, CancellationToken cancellationToken)
        => Task.CompletedTask;
}