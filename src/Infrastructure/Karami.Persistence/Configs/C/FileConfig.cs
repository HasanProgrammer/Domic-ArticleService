using Karami.Core.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using File = Karami.Domain.File.Entities.File;

namespace Karami.Persistence.Configs.C;

public class FileConfig : BaseEntityConfig<File, string>
{
    public override void Configure(EntityTypeBuilder<File> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/

        //Configs
        
        builder.ToTable("Files");

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
        
        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasOne(file => file.Article).WithMany(article => article.Files).HasForeignKey(file => file.ArticleId);
    }
}