using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Article.Contracts.Interfaces;

public interface IArticleCommandRepository : ICommandRepository<Entities.Article, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Entities.Article> FindByTitleAsync(string title, CancellationToken cancellationToken)
        => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<Entities.Article>> FindByCategoryIdEagerLoadingAsync(string categoryId,
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<Entities.Article> FindByUserId(string userId) => throw new NotImplementedException();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<Entities.Article>> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}