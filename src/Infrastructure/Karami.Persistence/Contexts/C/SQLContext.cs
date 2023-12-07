using Karami.Core.Domain.Entities;
using Karami.Core.Persistence.Configs;
using Karami.Domain.Article.Entities;
using Karami.Persistence.Configs.C;
using Microsoft.EntityFrameworkCore;
using File = Karami.Domain.File.Entities.File;

namespace Karami.Persistence.Contexts.C;

/*Setting*/
public partial class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        
    }
}

/*Entity*/
public partial class SQLContext
{
    public DbSet<Event> Events     { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<File> Files       { get; set; }
}

/*Config*/
public partial class SQLContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new EventConfig());
        builder.ApplyConfiguration(new ArticleConfig());
        builder.ApplyConfiguration(new FileConfig());
    }
}