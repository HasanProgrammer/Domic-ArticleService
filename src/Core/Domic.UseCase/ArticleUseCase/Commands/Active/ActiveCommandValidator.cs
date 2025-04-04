using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommandValidator(IArticleCommandRepository articleCommandRepository) : IValidator<ActiveCommand>
{
    public async Task<object> ValidateAsync(ActiveCommand input, CancellationToken cancellationToken)
    {
        var article = await articleCommandRepository.FindByIdAsync(input.Id, cancellationToken);

        if (article is null)
            throw new UseCaseException(
                string.Format("موجودیتی با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
            );

        return article;
    }
}