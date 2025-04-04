#pragma warning disable CS0649

using Domic.Domain.File.ValueObjects;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;

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
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="serializer"></param>
    /// <param name="identityUser"></param>
    /// <param name="articleId"></param>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="extension"></param>
    public File(IDateTime dateTime, IGlobalUniqueIdGenerator globalUniqueIdGenerator, ISerializer serializer,
        IIdentityUser identityUser, string articleId, string path, string fileName, string extension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          =   globalUniqueIdGenerator.GetRandom(6);
        ArticleId   = articleId;
        Path        = new ValueObjects.Path(path);
        Name        = new Name(fileName);
        Extension   = new Extension(extension);
        IsActive    = IsActive.Active;

        //audit
        CreatedBy   = identityUser.GetIdentity();
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
    }

    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    public void Delete(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime = DateTime.Now;
        
        IsDeleted = IsDeleted.Delete;

        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, dateTime.ToPersianShortDate(nowDateTime));
    }
}