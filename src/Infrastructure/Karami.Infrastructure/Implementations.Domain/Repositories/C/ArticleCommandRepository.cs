using Karami.Domain.Article.Contracts.Interfaces;
using Karami.Domain.Article.Entities;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class ArticleCommandRepository : IArticleCommandRepository
{
    private readonly SQLContext _sqlContext;

    public ArticleCommandRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class ArticleCommandRepository
{
    public async Task AddAsync(Article entity, CancellationToken cancellationToken) 
        => await _sqlContext.Articles.AddAsync(entity);

    public void RemoveRange(IEnumerable<Article> entities) => _sqlContext.Articles.RemoveRange(entities);

    public void Change(Article entity) => _sqlContext.Articles.Update(entity);
}

//Query
public partial class ArticleCommandRepository
{
    public async Task<Article> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _sqlContext.Articles.FirstOrDefaultAsync(article => article.Id.Equals(id), cancellationToken);

    public async Task<Article> FindByTitleAsync(string title, CancellationToken cancellationToken)
        => await _sqlContext.Articles.FirstOrDefaultAsync(article => article.Title.Value.Equals(title));

    public async Task<List<Article>> FindByCategoryIdEagerLoadingAsync(string categoryId,
        CancellationToken cancellationToken
    ) => await _sqlContext.Articles.Where(article => article.CategoryId.Equals(categoryId))
                                   .Include(article => article.Files)
                                   .ToListAsync(cancellationToken);

    public List<Article> FindByUserId(string userId)
        => _sqlContext.Articles.Where(article => article.CreatedBy.Equals(userId)).ToList();
}