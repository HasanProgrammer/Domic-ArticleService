using Karami.Domain.File.Contracts.Interfaces;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using File = Karami.Domain.File.Entities.File;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class FileCommandRepository : IFileCommandRepository
{
    private readonly SQLContext _sqlContext;

    public FileCommandRepository(SQLContext sqlContext) => _sqlContext = sqlContext;
}

//Transaction
public partial class FileCommandRepository
{
    public async Task AddAsync(File entity, CancellationToken cancellationToken)
        => await _sqlContext.Files.AddAsync(entity);

    public void RemoveRange(IEnumerable<File> entities) => _sqlContext.Files.RemoveRange(entities);
}

//Query
public partial class FileCommandRepository
{
    public async Task<File> FindByArticleIdAsync(string id, CancellationToken cancellationToken)
        => await _sqlContext.Files.FirstOrDefaultAsync(file => file.ArticleId.Equals(id), cancellationToken);
}