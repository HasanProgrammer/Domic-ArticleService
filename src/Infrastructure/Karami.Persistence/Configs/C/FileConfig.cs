using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using File = Karami.Domain.File.Entities.File;

namespace Karami.Persistence.Configs.C;

public class FileConfig : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        //PrimaryKey
        
        builder.HasKey(file => file.Id);

        builder.ToTable("Files");
        
        /*-----------------------------------------------------------*/

        //Property

        builder.Property(file => file.ArticleId).IsRequired();
        
        builder.OwnsOne(file => file.Path)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasColumnName("Path");
        
        builder.OwnsOne(file => file.Name)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasColumnName("Name");
        
        builder.OwnsOne(file => file.Extension)
               .Property(vo => vo.Value)
               .IsRequired()
               .HasColumnName("Extension");
        
        builder.OwnsOne(file => file.CreatedAt, createdAt => {
            createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
            createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(file => file.UpdatedAt, updatedAt => {
            updatedAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
            updatedAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasOne(file => file.Article).WithMany(article => article.Files).HasForeignKey(file => file.ArticleId);
    }
}