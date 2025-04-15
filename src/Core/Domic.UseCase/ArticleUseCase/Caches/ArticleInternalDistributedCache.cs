using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.UseCase.ArticleUseCase.DTOs;
using Domic.UseCase.FileUseCase.DTOs;

namespace Domic.UseCase.ArticleUseCase.Caches;

public class ArticleInternalDistributedCache(IArticleCommandRepository articleCommandRepository) 
    : IInternalDistributedCacheHandler<List<ArticleDto>>
{
    [Config(Key = Cache.Articles, Ttl = 24*60)]
    public Task<List<ArticleDto>> SetAsync(CancellationToken cancellationToken) 
        => articleCommandRepository.FindAllByProjectionAsync(article => new ArticleDto {
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
            Files = article.Files.Select(file => new FileDto {
                Id        = file.Id         ,
                Path      = file.Path.Value ,
                Name      = file.Name.Value ,
                Extension = file.Extension.Value 
            })
        }, cancellationToken: cancellationToken);
}