using System.Diagnostics;
using System.Diagnostics.Metrics;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FP.gRPCdotnet.Workshop.Server.Services;

public class MeticsInterceptor  : Interceptor
{
    internal static readonly Meter Metrics = new("gRPCMetrics");

    private readonly Counter<int> _grpcCallCounter = Metrics.CreateCounter<int>("grpc_requests_received_total",
        description:"Number of gRPC requests received (including those currently being processed).");

    private readonly Histogram<double> _grpcServerDuration = Metrics.CreateHistogram<double>("grpc_requests_duration", unit: "ms");
        

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var methodInfos = context.Method.Split("/", StringSplitOptions.RemoveEmptyEntries);

        var serviceName = methodInfos.Length > 0 ? methodInfos[0] : string.Empty;
        var methodName = methodInfos.Length == 2 ? methodInfos[1] : string.Empty;
        return await CreateMetrics<TResponse>(serviceName, methodName, () => continuation(request, context));
    }

    private async Task<TResponse> CreateMetrics<TResponse>(string service, string method,
        Func<Task<TResponse>> call)
    {
        var tags = new TagList();

        if (!string.IsNullOrEmpty(service))
        {
            tags.Add("service", service);
        }

        if (!string.IsNullOrEmpty(method))
        {
            tags.Add("service", method);
        }

        var result = await _grpcServerDuration.MeterDuration(call, tags);
        _grpcCallCounter.Add(1, tags);
        return result;
    }
    
}