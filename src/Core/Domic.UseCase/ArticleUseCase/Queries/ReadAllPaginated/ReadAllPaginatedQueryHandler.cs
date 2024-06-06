using Domic.UseCase.ArticleUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<ArticlesViewModel>>
{
    private readonly IInternalDistributedCacheMediator _distributedCacheMediator;

    public ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator distributedCacheMediator) 
        => _distributedCacheMediator = distributedCacheMediator;

    [WithValidation]
    public async Task<PaginatedCollection<ArticlesViewModel>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var result = await _distributedCacheMediator.GetAsync<List<ArticlesViewModel>>(cancellationToken);

        return result.ToPaginatedCollection(
            result.Count, query.CountPerPage ?? default, query.PageNumber ?? default, true
        );
    }
}