using Domic.Core.Infrastructure.Extensions;
using Domic.Core.WebAPI.Extensions;
using Domic.WebAPI.EntryPoints.GRPCs;
using Domic.WebAPI.Frameworks.Extensions;
using Domic.Persistence.Contexts.C;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterELK();
builder.RegisterEntityFrameworkCoreCommand<SQLContext, string>();
builder.RegisterEntityFrameworkCoreQuery<SQLContext>();
builder.RegisterCommandRepositories();
builder.RegisterDistributedCaching();
builder.RegisterCommandQueryUseCases();
builder.RegisterGrpcServer();
builder.RegisterMessageBroker();
builder.RegisterEventsPublisher();
builder.RegisterEventsSubscriber();
builder.RegisterServices();

builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

#endregion

/*-------------------------------------------------------------------*/

WebApplication application = builder.Build();

/*-------------------------------------------------------------------*/

//Primary processing

application.Services.AutoMigration<SQLContext>();

/*-------------------------------------------------------------------*/

#region Middleware

if (application.Environment.IsProduction())
{
    application.UseHsts();
    application.UseHttpsRedirection();
}

application.UseRouting();

application.UseEndpoints(endpoints => {

    endpoints.HealthCheck(application.Services);
    
    #region GRPC's Services

    endpoints.MapGrpcService<ArticleRPC>();

    #endregion

});

#endregion

/*-------------------------------------------------------------------*/

//HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

application.Run();

/*-------------------------------------------------------------------*/

//For Integration Test

public partial class Program;