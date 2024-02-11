using Grpc.Core;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.Article.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.WebAPI.Extensions;
using Domic.UseCase.ArticleUseCase.Commands.Active;
using Domic.UseCase.ArticleUseCase.Commands.CheckExist;
using Domic.UseCase.ArticleUseCase.Commands.Create;
using Domic.UseCase.ArticleUseCase.Commands.Delete;
using Domic.UseCase.ArticleUseCase.Commands.InActive;
using Domic.UseCase.ArticleUseCase.Commands.Update;
using Domic.UseCase.ArticleUseCase.DTOs.ViewModels;
using Domic.UseCase.ArticleUseCase.Queries.ReadAllPaginated;
using Domic.WebAPI.Frameworks.Extensions.Mappers.ArticleMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class ArticleRPC : ArticleService.ArticleServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;

    public ArticleRPC(IMediator mediator, IConfiguration configuration)
    {
        _mediator      = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CheckExistResponse> CheckExist(CheckExistRequest request, ServerCallContext context)
    {
        //@ToDo Should be using [ ToCommand ] insted [ ToQuery ]
        var query = request.ToQuery<CheckExistCommand>();
            
        var result =
            await _mediator.DispatchAsync<bool>(query, context.CancellationToken);

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
            await _mediator.DispatchAsync<PaginatedCollection<ArticlesViewModel>>(query, context.CancellationToken);

        return result.ToRpcResponse<ReadAllPaginatedResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<CreateCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<CreateResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<UpdateCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<UpdateResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ActiveResponse> Active(ActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<ActiveCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<ActiveResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<InActiveResponse> InActive(InActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<InActiveCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<InActiveResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<DeleteCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<DeleteResponse>(_configuration);
    }
}