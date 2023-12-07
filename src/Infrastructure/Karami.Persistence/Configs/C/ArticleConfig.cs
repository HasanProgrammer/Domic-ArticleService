using Karami.Core.Domain.Enumerations;
using Karami.Domain.Article.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class ArticleConfig : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        //PrimaryKey
        
        builder.HasKey(article => article.Id);

        builder.ToTable("Articles");
        
        /*-----------------------------------------------------------*/

        //Property

        builder.Property(article => article.UserId)    .IsRequired();
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
        
        builder.OwnsOne(article => article.CreatedAt, createdAt => {
            createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
            createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(article => article.UpdatedAt, updatedAt => {
            updatedAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
            updatedAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });
        
        builder.Property(article => article.IsActive)
               .HasConversion(new EnumToNumberConverter<IsActive , int>())
               .IsRequired();
        
        builder.Property(article => article.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted , int>())
               .IsRequired();

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasMany(article => article.Files).WithOne(file => file.Article).HasForeignKey(file => file.ArticleId);
        
        /*-----------------------------------------------------------*/
        
        //Query

        builder.HasQueryFilter(article => article.IsDeleted == IsDeleted.UnDelete);
    }
}