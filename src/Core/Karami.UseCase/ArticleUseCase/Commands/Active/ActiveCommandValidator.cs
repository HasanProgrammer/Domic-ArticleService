using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Article.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommandValidator : IValidator<ActiveCommand>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ActiveCommandValidator(IArticleCommandRepository articleCommandRepository) 
        => _articleCommandRepository = articleCommandRepository;

    public async Task<object> ValidateAsync(ActiveCommand input, CancellationToken cancellationToken)
    {
        var article = await _articleCommandRepository.FindByIdAsync(input.TargetId, cancellationToken);

        if (article is null)
            throw new UseCaseException(
                string.Format("موجودیتی با شناسه {0} وجود خارجی ندارد !", input.TargetId ?? "_خالی_")
            );

        return article;
    }
}