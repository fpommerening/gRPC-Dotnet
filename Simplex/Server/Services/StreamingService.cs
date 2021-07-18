using System;
using System.Threading.Tasks;
using FP.gRPCdoetnet.Streaming.Simplex.Contract;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace FP.gRPCdoetnet.Streaming.Simplex.Server.Services
{
    public class StreamingService : StreamingServices.StreamingServicesBase
    {
        private readonly ILogger<StreamingService> _logger;

        public StreamingService(ILogger<StreamingService> logger)
        {
            _logger = logger;
        }

        public override async Task<Empty> SendLog(IAsyncStreamReader<LogRequest> requestStream, ServerCallContext context)
        {
            try
            {
                // need c# 8
                await foreach (var request in requestStream.ReadAllAsync())
                {
                    _logger.LogInformation($"{request.Timestamp.ToDateTime():G} - {request.Message}");
                }
                
            }
            catch (OperationCanceledException e)
            {
                // Operation was canceled
            }
            catch (RpcException rpcEx)
            {
                if (rpcEx.StatusCode == StatusCode.Cancelled)
                {
                    // Operation was canceled
                }
                else
                {
                    _logger.LogError(rpcEx, "Unexpected error on receive logs");
                }
            }
            return new Empty();
        }

        public override async Task GetTime(GetTimeRequest request, IServerStreamWriter<GetTimeResponse> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"Sending {request.Count} responses to {request.Name}");

            for (var i = 0; i < request.Count; i++)
            {
                await responseStream.WriteAsync(new GetTimeResponse {Timestamp = DateTime.UtcNow.ToTimestamp()});
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
