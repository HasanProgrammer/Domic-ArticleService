using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommandValidator : IValidator<CreateCommand>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public CreateCommandValidator(IArticleCommandRepository articleCommandRepository) 
        => _articleCommandRepository = articleCommandRepository;

    public async Task<object> ValidateAsync(CreateCommand input, CancellationToken cancellationToken)
    {
        var articleTarget =
            await _articleCommandRepository.FindByTitleAsync(input.Title, cancellationToken);
        
        if (articleTarget is not null)
            throw new UseCaseException(
                string.Format("مقاله ای با عنوان {0} در سامانه موجود است", input.Title) 
            );

        return default;
    }
}