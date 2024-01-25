using Karami.Core.Infrastructure.Extensions;
using Karami.Core.WebAPI.Extensions;
using Karami.WebAPI.EntryPoints.GRPCs;
using Karami.WebAPI.Frameworks.Extensions;

using C_SQLContext = Karami.Persistence.Contexts.C.SQLContext;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterCommandSqlServer<C_SQLContext>();
builder.RegisterCommandRepositories();
builder.RegisterCaching();
builder.RegisterMessageBroker();
builder.RegisterJobs();
builder.RegisterCommandQueryUseCases();
builder.RegisterGrpcServer();
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

application.Services.AutoMigration<C_SQLContext>();

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

public partial class Program {}