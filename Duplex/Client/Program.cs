﻿using FP.gRPCdotnet.Streaming.Duplex.Contract;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

static async Task SendMessage(AsyncDuplexStreamingCall<EchoMessage, EchoMessage> streamingCall)
{
    for (var i = 0; i < 10; i++)
    {
        await streamingCall.RequestStream.WriteAsync(new EchoMessage
        {
            Content = $"Hallo server {i}",
            Timestamp = DateTime.UtcNow.ToTimestamp()
        });
        await Task.Delay(500);
    }

    await streamingCall.RequestStream.CompleteAsync();
}

static async Task ReceiveEcho(AsyncDuplexStreamingCall<EchoMessage, EchoMessage> streamingCall)
{
    await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"{response.Timestamp.ToDateTime():G}: {response.Content}");
    }
}

var channel = GrpcChannel.ForAddress("http://localhost:5002");
var client = new StreamingServices.StreamingServicesClient(channel);
            
await Task.Delay(7500);

try
{
    using (var streamingCall = client.SendMultiEcho())
    {
        var sendTask = SendMessage(streamingCall);
        var receiveTask = ReceiveEcho(streamingCall);
        await Task.WhenAll(sendTask, receiveTask);
    }
}
catch (OperationCanceledException)
{
    // Server close streaming
}
catch (Exception e)
{
    Console.WriteLine($"Unexpected error on send messages \n {e}");
}

