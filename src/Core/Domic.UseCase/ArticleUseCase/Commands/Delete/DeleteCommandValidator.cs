using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Delete;

public class DeleteCommandValidator : IValidator<DeleteCommand>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public DeleteCommandValidator(IArticleCommandRepository articleCommandRepository) 
        => _articleCommandRepository = articleCommandRepository;

    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var article = await _articleCommandRepository.FindByIdAsync(input.TargetId, cancellationToken);

        if (article is null)
            throw new UseCaseException(
                string.Format("موجودیتی با شناسه {0} وجود خارجی ندارد !", input.TargetId ?? "_خالی_")
            );

        return article;
    }
}