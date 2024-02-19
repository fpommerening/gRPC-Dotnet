using FP.gRPCdotnet.Workshop.Server.Business;
using FP.gRPCdotnet.Workshop.Server.Services;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddOpenTelemetry();


// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton(new WorkshopRepository());

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();














