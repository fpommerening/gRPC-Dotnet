using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FP.gRPCdoetnet.Streaming.Duplex.Contract;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace FP.gRPCdoetnet.Streaming.Simplex.Server.Services
{
    public class StreamingService : StreamingServices.StreamingServicesBase
    {
        private readonly ILogger<StreamingService> _logger;
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        public StreamingService(ILogger<StreamingService> logger)
        {
            _logger = logger;
        }

        public override async Task SendMultiEcho(IAsyncStreamReader<EchoMessage> requestStream, IServerStreamWriter<EchoMessage> responseStream, ServerCallContext context)
        {
            var receiveTask =  ReceiveMessageAsync(requestStream, context);

            var sendTask = SendMessageFromQueueAsync(responseStream, context);

            await Task.WhenAll(receiveTask, sendTask);
        }

        private async Task SendMessageFromQueueAsync(IServerStreamWriter<EchoMessage> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                if (_queue.IsEmpty) continue;
                if (_queue.TryDequeue(out var msg))
                {
                    await responseStream.WriteAsync(new EchoMessage
                    {
                        Content = $"echo {msg}",
                        Timestamp = DateTime.UtcNow.ToTimestamp()
                    });
                }
            }
        }

        private async Task ReceiveMessageAsync(IAsyncStreamReader<EchoMessage> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var msg = requestStream.Current;
                _logger.LogInformation($"getting message '{msg.Content}' with timestamp {msg.Timestamp.ToDateTime():G}");
                _queue.Enqueue(msg.Content);
            }
        }
    }
}
