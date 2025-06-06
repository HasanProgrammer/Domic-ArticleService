﻿using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Article.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Delete;

public class DeleteCommandValidator(IArticleCommandRepository articleCommandRepository) : IValidator<DeleteCommand>
{
    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var article = await articleCommandRepository.FindByIdAsync(input.Id, cancellationToken);

        if (article is null)
            throw new UseCaseException(
                string.Format("موجودیتی با شناسه {0} یافت نشد !", input.Id ?? "_خالی_")
            );

        return article;
    }
}