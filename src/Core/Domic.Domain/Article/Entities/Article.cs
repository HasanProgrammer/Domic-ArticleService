#pragma warning disable CS0649

using Domic.Domain.Article.Events;
using Domic.Domain.Article.ValueObjects;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;

namespace Domic.Domain.Article.Entities;

public class Article : Entity<string>
{
    public string CategoryId { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Value Objects
    
    public Title Title     { get; private set; }
    public Summary Summary { get; private set; }
    public Body Body       { get; private set; }

    /*---------------------------------------------------------------*/
    
    //Relations
    
    public ICollection<File.Entities.File> Files { get; set; }

    /*---------------------------------------------------------------*/

    //EF Core
    public Article() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="serializer"></param>
    /// <param name="identityUser"></param>
    /// <param name="categoryId"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public Article(IDateTime dateTime, IGlobalUniqueIdGenerator globalUniqueIdGenerator, ISerializer serializer,
        IIdentityUser identityUser, string categoryId, string title, string summary, string body, string fileId, 
        string filePath, string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = globalUniqueIdGenerator.GetRandom(6);
        CategoryId  = categoryId;
        IsActive    = IsActive.Active;
        Title       = new Title(title);
        Summary     = new Summary(summary);
        Body        = new Body(body);
        
        //audit
        CreatedBy   = identityUser.GetIdentity();
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
        
        AddEvent(
            new ArticleCreated {
                Id                    = Id                 ,
                CategoryId            = categoryId         ,
                Title                 = title              ,
                Summary               = summary            ,
                Body                  = body               ,
                FileId                = fileId             ,
                FilePath              = filePath           ,
                FileName              = fileName           ,
                FileExtension         = fileExtension      ,
                CreatedBy             = CreatedBy          ,
                CreatedRole           = CreatedRole        ,
                CreatedAt_EnglishDate = nowDateTime        ,
                CreatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="categoryId"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public void Change(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer,
        string categoryId, string title, string summary, string body, string fileId, string filePath, 
        string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        CategoryId  = categoryId;
        Title       = new Title(title);
        Summary     = new Summary(summary);
        Body        = new Body(body);
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleUpdated {
                Id                    = Id            ,
                CategoryId            = categoryId    ,
                Title                 = title         ,
                Summary               = summary       ,
                Body                  = body          ,
                FileId                = fileId        ,
                FileName              = fileName      ,
                FileExtension         = fileExtension ,
                FilePath              = filePath      ,
                UpdatedBy             = UpdatedBy     ,
                UpdatedRole           = UpdatedRole   ,
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    /// <param name="raiseEvent"></param>
    public void Active(IDateTime dateTime, string updatedBy, string updatedRole, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsActive = IsActive.Active;
        
        //audit
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        if(raiseEvent)
            AddEvent(
                new ArticleActived {
                    Id                    = Id          ,
                    UpdatedBy             = UpdatedBy   ,
                    UpdatedRole           = UpdatedRole ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    public void Active(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsActive = IsActive.Active;
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleActived {
                Id                    = Id                    ,
                UpdatedBy             = UpdatedBy             ,
                UpdatedRole           = UpdatedRole           ,
                UpdatedAt_EnglishDate = nowDateTime           ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    /// <param name="raiseEvent"></param>
    public void InActive(IDateTime dateTime, string updatedBy, string updatedRole, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsActive = IsActive.InActive;
        
        //audit
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new ArticleInActived {
                    Id                    = Id                    ,
                    UpdatedBy             = UpdatedBy             ,
                    UpdatedRole           = UpdatedRole           ,
                    UpdatedAt_EnglishDate = nowDateTime           ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="raiseEvent"></param>
    public void InActive(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsActive    = IsActive.InActive;
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleInActived {
                Id                    = Id                    ,
                UpdatedBy             = UpdatedBy             ,
                UpdatedRole           = UpdatedRole           ,
                UpdatedAt_EnglishDate = nowDateTime           ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
            
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="raiseEvent"></param>
    public void Delete(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsDeleted   = IsDeleted.Delete;
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new ArticleDeleted {
                    Id                    = Id                    ,
                    UpdatedBy             = UpdatedBy             ,
                    UpdatedRole           = UpdatedRole           ,
                    UpdatedAt_EnglishDate = nowDateTime           ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
}