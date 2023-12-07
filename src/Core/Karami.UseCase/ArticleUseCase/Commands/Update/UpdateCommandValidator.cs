using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Article.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommandValidator : IValidator<UpdateCommand>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public UpdateCommandValidator(IArticleCommandRepository articleCommandRepository) 
        => _articleCommandRepository = articleCommandRepository;

    public async Task<object> ValidateAsync(UpdateCommand input, CancellationToken cancellationToken)
    {
        var targetArticle = 
            await _articleCommandRepository.FindByIdAsync(input.TargetId, cancellationToken);

        if (targetArticle is null)
            throw new UseCaseException(
                string.Format("مقاله ای با شناسه {0} وجود خارجی ندارد !", targetArticle.Id ?? "_خالی_")
            );

        if (!targetArticle.Title.Value.Equals(input.Title))
        {
            var articleByTitle = await _articleCommandRepository.FindByTitleAsync(input.Title, cancellationToken);
            
            if(articleByTitle is not null)
                throw new UseCaseException(
                    string.Format("مقاله ای با عنوان {0} در سامانه موجود است", input.Title) 
                );
        }

        return targetArticle;
    }
}