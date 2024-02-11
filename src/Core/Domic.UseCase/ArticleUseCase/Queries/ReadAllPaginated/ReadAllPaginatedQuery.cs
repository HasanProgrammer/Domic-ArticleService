using Domic.UseCase.ArticleUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<ArticlesViewModel>>
{
    
}