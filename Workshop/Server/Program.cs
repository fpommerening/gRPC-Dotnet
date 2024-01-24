using FP.gRPCdotnet.Workshop.Server.Business;
using FP.gRPCdotnet.Workshop.Server.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add(typeof(RpcExceptionWrapperInterceptor));
    // options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton(new WorkshopRepository());

var app = builder.Build();

app.UseGrpcMetrics();
// Configure the HTTP request pipeline.
app.MapGrpcService<WorkshopService>();
app.MapMetrics();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();














