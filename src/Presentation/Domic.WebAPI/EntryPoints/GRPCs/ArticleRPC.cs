using Grpc.Core;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.Article.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.ArticleUseCase.Commands.Active;
using Domic.UseCase.ArticleUseCase.Commands.CheckExist;
using Domic.UseCase.ArticleUseCase.Commands.Create;
using Domic.UseCase.ArticleUseCase.Commands.Delete;
using Domic.UseCase.ArticleUseCase.Commands.InActive;
using Domic.UseCase.ArticleUseCase.Commands.Update;
using Domic.UseCase.ArticleUseCase.Queries.ReadAllPaginated;
using Domic.WebAPI.Frameworks.Extensions.Mappers.ArticleMappers;
using Domic.UseCase.ArticleUseCase.DTOs;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class ArticleRPC(IMediator mediator, IConfiguration configuration) 
    : ArticleService.ArticleServiceBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CheckExistResponse> CheckExist(CheckExistRequest request, ServerCallContext context)
    {
        var query = request.ToQuery<CheckExistCommand>();
            
        var result =
            await mediator.DispatchAsync<bool>(query, context.CancellationToken);

        return result.ToRpcResponse<CheckExistResponse>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllPaginatedResponse> ReadAllPaginated(ReadAllPaginatedRequest request, 
        ServerCallContext context
    )
    {
        var query = request.ToQuery<ReadAllPaginatedQuery>();

        var result = 
            await mediator.DispatchAsync<PaginatedCollection<ArticleDto>>(query, context.CancellationToken);

        return result.ToRpcResponse<ReadAllPaginatedResponse>(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<CreateCommand>();
        
        var result = await mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<CreateResponse>(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<UpdateCommand>();
        
        var result = await mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<UpdateResponse>(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ActiveResponse> Active(ActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<ActiveCommand>();
        
        var result = await mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<ActiveResponse>(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<InActiveResponse> InActive(InActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<InActiveCommand>();
        
        var result = await mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<InActiveResponse>(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<DeleteCommand>();
        
        var result = await mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<DeleteResponse>(configuration);
    }
}