using FP.gRPCdotnet.Workshop.Server.Business;
using FP.gRPCdotnet.Workshop.Server.Services;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder.AddPrometheusExporter();
        meterProviderBuilder.AddMeter(MeticsInterceptor.Metrics.Name);
    });


// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<MeticsInterceptor>();
    options.Interceptors.Add<RpcExceptionWrapperInterceptor>();

    // options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton(new WorkshopRepository());

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();
// Configure the HTTP request pipeline.
app.MapGrpcService<WorkshopService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();














