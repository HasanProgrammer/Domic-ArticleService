#pragma warning disable CS0649

using Domic.Domain.File.ValueObjects;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;
using Path = Domic.Domain.File.ValueObjects.Path;

namespace Domic.Domain.File.Entities;

public class File : Entity<string>
{
    public string ArticleId { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Value Objects
    
    public ValueObjects.Path Path           { get; private set; }
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
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="articleId"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="extension"></param>
    public File(IDateTime dateTime, string id, string articleId, string createdBy, string createdRole,
        string path, string fileName, string extension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = id;
        ArticleId   = articleId;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        Path        = new ValueObjects.Path(path);
        Name        = new Name(fileName);
        Extension   = new Extension(extension);
        IsActive    = IsActive.Active;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
    }

    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    public void Delete(IDateTime dateTime, string updatedBy, string updatedRole)
    {
        var nowDateTime = DateTime.Now;
        
        IsDeleted   = IsDeleted.Delete;
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, dateTime.ToPersianShortDate(nowDateTime));
    }
}