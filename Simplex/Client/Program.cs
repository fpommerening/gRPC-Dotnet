using FP.gRPCdotnet.Streaming.Simplex.Contract;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

static async Task SendLogs(StreamingServices.StreamingServicesClient client)
{
    try
    {
        using (var streamingCall = client.SendLog())
        {
            for (var i = 0; i < 10; i++)
            {
                await streamingCall.RequestStream.WriteAsync(new LogRequest
                {
                    Message = $"Log {i}",
                    Timestamp = DateTime.UtcNow.ToTimestamp()
                });

            }

            await streamingCall.RequestStream.CompleteAsync();
            var result = await streamingCall.ResponseAsync;
        }
    }
    catch (OperationCanceledException)
    {
        // Server close streaming
    }
    catch (Exception e)
    {
        Console.WriteLine($"Unexpected error on send logs \n {e}");
    }
}

static async Task GetTime(StreamingServices.StreamingServicesClient client)
  {
      try
      {
          var request = new GetTimeRequest()
          {
              Count = 10,
              Name = "Frank"
          };
          using (var streamingCall = client.GetTime(request))
          {
              await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
              {
                  Console.WriteLine($"Time from server {response.Timestamp.ToDateTime():G}");
              }
          }
      }
      catch (OperationCanceledException)
      {
          // Server close streaming
      }
      catch (Exception e)
      {
          Console.WriteLine($"Unexpected error on receive time \n {e}");
      }
  }

var channel = GrpcChannel.ForAddress("http://localhost:5002");
var client = new StreamingServices.StreamingServicesClient(channel);
await SendLogs(client);
//await GetTime(client);
