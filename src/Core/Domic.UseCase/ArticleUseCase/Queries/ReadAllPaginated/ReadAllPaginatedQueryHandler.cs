using Domic.UseCase.ArticleUseCase.DTOs;
using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator distributedCacheMediator) 
    : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<ArticleDto>>
{
    [WithValidation]
    public async Task<PaginatedCollection<ArticleDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var result = await distributedCacheMediator.GetAsync<List<ArticleDto>>(cancellationToken);

        return result.ToPaginatedCollection(
            result.Count, query.CountPerPage ?? default, query.PageNumber ?? default, true
        );
    }
}