#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;
using Karami.Domain.File.ValueObjects;
using Path = Karami.Domain.File.ValueObjects.Path;

namespace Karami.Domain.File.Entities;

public class File : Entity<string>
{
    public string ArticleId { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Value Objects
    
    public Path Path           { get; private set; }
    public Name Name           { get; private set; }
    public Extension Extension { get; private set; }

    /*---------------------------------------------------------------*/
    
    //Relations
    
    public Article.Entities.Article Article { get; set; }

    /*---------------------------------------------------------------*/

    //EF Core
    public File() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="articleId"></param>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="extension"></param>
    public File(IDotrisDateTime dotrisDateTime, string id, string articleId, string path, string fileName, string extension)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        Id        = id;
        ArticleId = articleId;
        Path      = new Path(path);
        Name      = new Name(fileName);
        Extension = new Extension(extension);
        IsActive  = IsActive.Active;
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
    }

    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    public void Delete(IDotrisDateTime dotrisDateTime)
    {
        var nowDateTime = DateTime.Now;
        
        IsDeleted = IsDeleted.Delete;
        UpdatedAt = new UpdatedAt(nowDateTime, dotrisDateTime.ToPersianShortDate(nowDateTime));
    }
}