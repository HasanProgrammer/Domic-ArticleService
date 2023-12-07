using Karami.Core.Common.ClassExtensions;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.Grpc.Article;
using Karami.Core.Infrastructure.Extensions;
using Karami.UseCase.ArticleUseCase.DTOs.ViewModels;

namespace Karami.WebAPI.Frameworks.Extensions.Mappers.ArticleMappers;

//Query
public static partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this bool result)
    {
        object Response = null;

        if(typeof(T) == typeof(CheckExistResponse))
            Response = new CheckExistResponse { Result = result };

        return (T) Response;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="models"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this PaginatedCollection<ArticlesViewModel> models, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadAllPaginatedResponse))
        {
            Response = new ReadAllPaginatedResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = new ReadAllPaginatedResponseBody {
                    Articles = models.Serialize()
                }
            };
        }

        return (T) Response;
    }
}

//Command
public partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this string response, IConfiguration configuration)
    {
        object Response = null;
        
        if (typeof(T) == typeof(CreateResponse))
        {
            Response = new CreateResponse {
                Code    = configuration.GetSuccessCreateStatusCode() ,
                Message = configuration.GetSuccessCreateMessage()    ,
                Body    = new CreateResponseBody { ArticleId = response }
            };
        }
        else if (typeof(T) == typeof(UpdateResponse))
        {
            Response = new UpdateResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new UpdateResponseBody { ArticleId = response }
            };
        }
        else if (typeof(T) == typeof(ActiveResponse))
        {
            Response = new ActiveResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new ActiveResponseBody { ArticleId = response }
            };
        }
        else if (typeof(T) == typeof(InActiveResponse))
        {
            Response = new InActiveResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new InActiveResponseBody { ArticleId = response }
            };
        }
        else if (typeof(T) == typeof(DeleteResponse))
        {
            Response = new DeleteResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new DeleteResponseBody { ArticleId = response }
            };
        }

        return (T)Response;
    }
}