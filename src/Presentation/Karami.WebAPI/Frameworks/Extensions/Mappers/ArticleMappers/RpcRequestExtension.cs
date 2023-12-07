using Karami.Core.Grpc.Article;
using Karami.UseCase.ArticleUseCase.Commands.Active;
using Karami.UseCase.ArticleUseCase.Commands.CheckExist;
using Karami.UseCase.ArticleUseCase.Commands.Create;
using Karami.UseCase.ArticleUseCase.Commands.Delete;
using Karami.UseCase.ArticleUseCase.Commands.InActive;
using Karami.UseCase.ArticleUseCase.Commands.Update;
using Karami.UseCase.ArticleUseCase.Queries.ReadAllPaginated;

namespace Karami.WebAPI.Frameworks.Extensions.Mappers.ArticleMappers;

//Query
public static partial class RpcRequestExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this CheckExistRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(CheckExistCommand))
        {
            Request = new CheckExistCommand {
                ArticleId = request.TargetId.Value
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadAllPaginatedRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadAllPaginatedQuery))
        {
            Request = new ReadAllPaginatedQuery {
                PageNumber   = request.PageNumber.Value,
                CountPerPage = request.CountPerPage.Value
            };
        }

        return (T)Request;
    }
}

//Command
public partial class RpcRequestExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this CreateRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(CreateCommand))
        {
            Request = new CreateCommand {
                Token         = token                     ,
                UserId        = request.UserId?.Value     ,
                CategoryId    = request.CategoryId?.Value ,
                Title         = request.Title?.Value      ,
                Summary       = request.Summary?.Value    ,
                Body          = request.Body?.Value       ,
                FilePath      = request.Image.Path?.Value ,
                FileName      = request.Image.Name?.Value ,
                FileExtension = request.Image.Extension?.Value
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this UpdateRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(UpdateCommand))
        {
            Request = new CreateCommand {
                Token         = token                     ,
                UserId        = request.UserId?.Value     ,
                CategoryId    = request.CategoryId?.Value ,
                Title         = request.Title?.Value      ,
                Summary       = request.Summary?.Value    ,
                Body          = request.Body?.Value       ,
                FilePath      = request.Image.Path?.Value ,
                FileName      = request.Image.Name?.Value ,
                FileExtension = request.Image.Extension?.Value
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this ActiveRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(ActiveCommand))
        {
            Request = new ActiveCommand {
                Token    = token ,
                TargetId = request.TargetId.Value
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this InActiveRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(InActiveCommand))
        {
            Request = new InActiveCommand {
                Token    = token ,
                TargetId = request.TargetId.Value
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this DeleteRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(DeleteCommand))
        {
            Request = new DeleteCommand {
                Token    = token ,
                TargetId = request.TargetId.Value
            };
        }

        return (T)Request;
    }
}