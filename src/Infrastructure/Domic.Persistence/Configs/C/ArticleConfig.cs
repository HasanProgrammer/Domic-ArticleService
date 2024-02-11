using Domic.Core.Persistence.Configs;
using Domic.Domain.Article.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.C;

public class ArticleConfig : BaseEntityConfig<Article, string>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/

        //Configs
        
        builder.ToTable("Articles");

        builder.Property(article => article.CategoryId).IsRequired();
        
        builder.OwnsOne(article => article.Title)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasMaxLength(200)
               .HasColumnName("Title");
        
        builder.OwnsOne(article => article.Summary)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasMaxLength(500)
               .HasColumnName("Summary");
        
        builder.OwnsOne(article => article.Body)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasColumnName("Body");

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasMany(article => article.Files).WithOne(file => file.Article).HasForeignKey(file => file.ArticleId);
    }
}