using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.CheckExist;

public class CheckExistCommandHandler : IQueryHandler<CheckExistCommand, bool>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public CheckExistCommandHandler(IArticleCommandRepository articleCommandRepository) 
        => _articleCommandRepository = articleCommandRepository;

    public async Task<bool> HandleAsync(CheckExistCommand command, CancellationToken cancellationToken)
    {
        var result = await _articleCommandRepository.FindByIdAsync(command.ArticleId, cancellationToken);

        return result is not null;
    }
}