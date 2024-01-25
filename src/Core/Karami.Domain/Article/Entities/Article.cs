#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;
using Karami.Domain.Article.Events;
using Karami.Domain.Article.ValueObjects;

namespace Karami.Domain.Article.Entities;

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
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="categoryId"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public Article(IDateTime dateTime, string id, string createdBy, string createdRole, string categoryId, 
        string title, string summary, string body, string fileId, string filePath, string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = id;
        CategoryId  = categoryId;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        Title       = new Title(title);
        Summary     = new Summary(summary);
        Body        = new Body(body);
        IsActive    = IsActive.Active;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleCreated {
                Id                    = Id                 ,
                CreatedBy             = createdBy          ,
                CreatedRole           = createdRole        ,
                CategoryId            = categoryId         ,
                Title                 = title              ,
                Summary               = summary            ,
                Body                  = body               ,
                FileId                = fileId             ,
                FilePath              = filePath           ,
                FileName              = fileName           ,
                FileExtension         = fileExtension      ,
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
    /// <param name="categoryId"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public void Change(IDateTime dateTime, string categoryId, string updatedBy, string updatedRole, string title, 
        string summary, string body, string fileId, string filePath, string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        CategoryId  = categoryId;
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        Title       = new Title(title);
        Summary     = new Summary(summary);
        Body        = new Body(body);
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleUpdated {
                Id                    = Id            ,
                CategoryId            = categoryId    ,
                UpdatedBy             = updatedBy     ,
                UpdatedRole           = updatedRole   ,
                Title                 = title         ,
                Summary               = summary       ,
                Body                  = body          ,
                FileId                = fileId        ,
                FileName              = fileName      ,
                FileExtension         = fileExtension ,
                FilePath              = filePath      ,
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
        
        IsActive    = IsActive.Active;
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new ArticleActived {
                    Id                    = Id          ,
                    UpdatedBy             = updatedBy   ,
                    UpdatedRole           = updatedRole ,
                    UpdatedAt_EnglishDate = nowDateTime ,
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
        
        IsActive    = IsActive.InActive;
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new ArticleInActived {
                    Id                    = Id          ,
                    UpdatedBy             = updatedBy   ,
                    UpdatedRole           = updatedRole ,
                    UpdatedAt_EnglishDate = nowDateTime ,
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
    public void Delete(IDateTime dateTime, string updatedBy, string updatedRole, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        IsDeleted   = IsDeleted.Delete;
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new ArticleDeleted {
                    Id                    = Id          ,
                    UpdatedBy             = updatedBy   ,
                    UpdatedRole           = updatedRole ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
}