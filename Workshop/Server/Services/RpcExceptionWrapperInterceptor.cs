using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FP.gRPCdotnet.Workshop.Server.Services
{
    public class RpcExceptionWrapperInterceptor : Interceptor
    {
        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return ConvertExceptions(() => continuation(requestStream, context));
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return ConvertExceptions(() => continuation(requestStream, responseStream, context));
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            return ConvertExceptions(() => continuation(request, responseStream, context));
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            return ConvertExceptions(() => continuation(request, context));
        }

        private async Task<TResponse> ConvertExceptions<TResponse>(Func<Task<TResponse>> call)
        {
            try
            {
                return await call();
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.ToString()));
            }
        }

        private async Task ConvertExceptions(Func<Task> call)
        {
            try
            {
                await call();
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.ToString()));
            }
        }
    }
}
