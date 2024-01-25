using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Enumerations;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Article.Contracts.Interfaces;
using Karami.UseCase.ArticleUseCase.DTOs.ViewModels;
using Karami.UseCase.FileUseCase.DTOs.ViewModels;

namespace Karami.UseCase.ArticleUseCase.Caches;

public class ArticleMemoryCache : IMemoryCacheSetter<List<ArticlesViewModel>>
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