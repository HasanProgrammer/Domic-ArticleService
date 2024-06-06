using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.UseCase.ArticleUseCase.DTOs.ViewModels;
using Domic.UseCase.FileUseCase.DTOs.ViewModels;

namespace Domic.UseCase.ArticleUseCase.Caches;

public class ArticleMemoryCache : IInternalDistributedCacheHandler<List<ArticlesViewModel>>
{
    private readonly IArticleCommandRepository _articleCommandRepository;

    public ArticleMemoryCache(IArticleCommandRepository articleCommandRepository)
        => _articleCommandRepository = articleCommandRepository;
    
    [Config(Key = Cache.Articles, Ttl = 24*60)]
    public async Task<List<ArticlesViewModel>> SetAsync(CancellationToken cancellationToken)
    {
        var query =
            await _articleCommandRepository.FindAllEagerLoadingWithOrderingAsync(Order.Date, false, cancellationToken);

        return query.Select(article => new ArticlesViewModel {
            Id                    = article.Id                          ,
            CreatedBy             = article.CreatedBy                   ,
            CategoryId            = article.CategoryId                  ,
            Title                 = article.Title.Value                 ,
            Summary               = article.Summary.Value               ,
            Body                  = article.Body.Value                  ,
            IsActive              = article.IsActive == IsActive.Active ,
            CreatedAt_EnglishDate = article.CreatedAt.EnglishDate       ,
            CreatedAt_PersianDate = article.CreatedAt.PersianDate       ,
            UpdatedAt_EnglishDate = article.UpdatedAt.EnglishDate       ,
            UpdatedAt_PersianDate = article.UpdatedAt.PersianDate       ,
            Files = article.Files.Select(file => new FilesViewModel {
                Id        = file.Id         ,
                Path      = file.Path.Value ,
                Name      = file.Name.Value ,
                Extension = file.Extension.Value 
            })
        }).ToList();
    }
}