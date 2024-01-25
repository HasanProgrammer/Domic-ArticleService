#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Article.Entities;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommandHandler : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;
    
    private readonly IDateTime                 _dateTime;
    private readonly ISerializer               _serializer;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IEventCommandRepository   _eventCommandRepository;
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveCommandHandler(IArticleCommandRepository articleCommandRepository,
        IEventCommandRepository eventCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dateTime                 = dateTime;
        _serializer               = serializer;
        _jsonWebToken             = jsonWebToken;
        _eventCommandRepository   = eventCommandRepository;
        _articleCommandRepository = articleCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var article = _validationResult as Article;

        var updatedBy = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        article.Active(_dateTime, updatedBy, updatedRole);

        _articleCommandRepository.Change(article);

        #region OutBox

        //ToDo : ( Tech Debt ) -> Should be used another way for handling [OutBox] pattern like using [Interceptor]
        var events = article.GetEvents.ToEntityOfEvent(_dateTime, _serializer, Service.ArticleService,
            Table.ArticleTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        //ToDo : ( Tech Debt ) -> Should be used [Add] insted [AddAsync]
        foreach (var @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);

        #endregion

        return article.Id;
    }
}