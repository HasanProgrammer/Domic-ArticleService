using Domic.Domain.File.Contracts.Interfaces;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using File = Domic.Domain.File.Entities.File;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class FileCommandRepository(SQLContext sqlContext) : IFileCommandRepository;

//Transaction
public partial class FileCommandRepository
{
    public Task AddAsync(File entity, CancellationToken cancellationToken)
    {
        sqlContext.Files.Add(entity);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(File entity, CancellationToken cancellationToken)
    {
        sqlContext.Files.Remove(entity);

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<File> entities, CancellationToken cancellationToken)
    {
        sqlContext.Files.RemoveRange(entities);

        return Task.CompletedTask;
    }
}

//Query
public partial class FileCommandRepository
{
    public Task<File> FindByArticleIdAsync(string id, CancellationToken cancellationToken)
        => sqlContext.Files.FirstOrDefaultAsync(file => file.ArticleId == id, cancellationToken);
}