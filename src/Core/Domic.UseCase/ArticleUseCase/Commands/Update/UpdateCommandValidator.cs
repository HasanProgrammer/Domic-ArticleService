using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommandValidator(IArticleCommandRepository articleCommandRepository) : IValidator<UpdateCommand>
{
    public async Task<object> ValidateAsync(UpdateCommand input, CancellationToken cancellationToken)
    {
        var targetArticle =
            await articleCommandRepository.FindByIdAsync(input.Id, cancellationToken);

        if (targetArticle is null)
            throw new UseCaseException(
                string.Format("مقاله ای با شناسه {0} یافت نشد !", targetArticle.Id ?? "_خالی_")
            );

        if (!targetArticle.Title.Value.Equals(input.Title))
        {
            var articleByTitle = await articleCommandRepository.FindByTitleAsync(input.Title, cancellationToken);
            
            if(articleByTitle is not null)
                throw new UseCaseException(
                    string.Format("مقاله ای با عنوان {0} در سامانه موجود است", input.Title) 
                );
        }

        return targetArticle;
    }
}