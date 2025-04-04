using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.CheckExist;

public class CheckExistCommandHandler(IArticleCommandRepository articleCommandRepository) : IQueryHandler<CheckExistCommand, bool>
{
    public async Task<bool> HandleAsync(CheckExistCommand command, CancellationToken cancellationToken)
    {
        var result = await articleCommandRepository.FindByIdAsync(command.Id, cancellationToken);

        return result is not null;
    }
}