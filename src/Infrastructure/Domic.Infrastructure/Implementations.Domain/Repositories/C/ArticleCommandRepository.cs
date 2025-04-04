using Domic.Core.Domain.Enumerations;
using Domic.Domain.Article.Contracts.Interfaces;
using Domic.Domain.Article.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class ArticleCommandRepository(SQLContext sqlContext) : IArticleCommandRepository
{
}

//Transaction
public partial class ArticleCommandRepository
{
    public Task AddAsync(Article entity, CancellationToken cancellationToken) 
    {
        sqlContext.Articles.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(Article entity, CancellationToken cancellationToken)
    {
        sqlContext.Articles.Update(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<Article> entities, CancellationToken cancellationToken)
    {
        sqlContext.Articles.RemoveRange(entities);

        return Task.CompletedTask;
    }
}

//Query
public partial class ArticleCommandRepository
{
    public Task<Article> FindByIdAsync(object id, CancellationToken cancellationToken)
        => sqlContext.Articles.FirstOrDefaultAsync(article => article.Id == id as string, cancellationToken);

    public Task<Article> FindByTitleAsync(string title, CancellationToken cancellationToken)
        => sqlContext.Articles.AsNoTracking().FirstOrDefaultAsync(article => article.Title.Value == title, cancellationToken);

    public Task<List<Article>> FindByCategoryIdEagerLoadingAsync(string categoryId,
        CancellationToken cancellationToken
    ) => sqlContext.Articles.Where(article => article.CategoryId == categoryId)
                            .Include(article => article.Files)
                            .ToListAsync(cancellationToken);

    public Task<List<Article>> FindByUserIdAsync(string userId, CancellationToken cancellationToken)
        => sqlContext.Articles.AsNoTracking().Where(article => article.CreatedBy == userId).ToListAsync(cancellationToken);

    public Task<List<Article>> FindAllEagerLoadingWithOrderingAsync(Order order, bool isAscending, CancellationToken cancellationToken)
    {
        var query = sqlContext.Articles.AsNoTracking();

        if (order == Order.Date)
            query = isAscending
                ? query.OrderBy(article => article.CreatedAt.EnglishDate.Value) 
                : query.OrderByDescending(article => article.CreatedAt.EnglishDate.Value);
        else if (order == Order.Id)
            query = isAscending
                ? query.OrderBy(article => article.Id)
                : query.OrderByDescending(article => article.Id);

        return query.Include(article => article.Files).ToListAsync(cancellationToken);    
    }
}