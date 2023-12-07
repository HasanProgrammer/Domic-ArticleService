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
    public string UserId     { get; private set; }
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
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="categoryId"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public Article(IDotrisDateTime dotrisDateTime, string id, string userId, string categoryId, string title, string summary, string body,
        string fileId, string filePath, string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        Id         = id;
        UserId     = userId;
        CategoryId = categoryId;
        Title      = new Title(title);
        Summary    = new Summary(summary);
        Body       = new Body(body);
        IsActive   = IsActive.Active;
        CreatedAt  = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt  = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new ArticleCreated {
                Id                    = Id                 ,
                UserId                = userId             ,
                CategoryId            = categoryId         ,
                Title                 = title              ,
                Summary               = summary            ,
                Body                  = body               ,
                FileId                = fileId             ,
                FilePath              = filePath           ,
                FileName              = fileName           ,
                FileExtension         = fileExtension      ,
                CreatedAt_EnglishDate = nowDateTime        ,
                UpdatedAt_EnglishDate = nowDateTime        ,
                UpdatedAt_PersianDate = nowPersianDateTime ,
                CreatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="categoryId"></param>
    /// <param name="title"></param>
    /// <param name="summary"></param>
    /// <param name="body"></param>
    /// <param name="fileId"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    public void Change(IDotrisDateTime dotrisDateTime, string categoryId, string title, string summary, string body,
        string fileId, string filePath, string fileName, string fileExtension
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        CategoryId = categoryId;
        Title      = new Title(title);
        Summary    = new Summary(summary);
        Body       = new Body(body);
        UpdatedAt  = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
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
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime 
            }
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="raiseEvent"></param>
    public void Active(IDotrisDateTime dotrisDateTime, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        
        IsActive  = IsActive.Active;
        UpdatedAt = new UpdatedAt(nowDateTime, dotrisDateTime.ToPersianShortDate(nowDateTime));
        
        if(raiseEvent)
            AddEvent(
                new ArticleActived {
                    Id                    = Id          ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = dotrisDateTime.ToPersianShortDate(nowDateTime)
                }
            );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="raiseEvent"></param>
    public void InActive(IDotrisDateTime dotrisDateTime, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        
        IsActive  = IsActive.InActive;
        UpdatedAt = new UpdatedAt(nowDateTime, dotrisDateTime.ToPersianShortDate(nowDateTime));
        
        if(raiseEvent)
            AddEvent(
                new ArticleInActived {
                    Id                    = Id          ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = dotrisDateTime.ToPersianShortDate(nowDateTime)
                }
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="raiseEvent"></param>
    public void Delete(IDotrisDateTime dotrisDateTime, bool raiseEvent = true)
    {
        var nowDateTime = DateTime.Now;
        
        IsDeleted = IsDeleted.Delete;
        UpdatedAt = new UpdatedAt(nowDateTime, dotrisDateTime.ToPersianShortDate(nowDateTime));
        
        if(raiseEvent)
            AddEvent(
                new ArticleDeleted {
                    Id                    = Id          ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = dotrisDateTime.ToPersianShortDate(nowDateTime)
                }
            );
    }
}