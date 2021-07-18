using System;
using FP.gRPCdoetnet.Workshop.Contract;
using Grpc.Net.Client;

namespace FP.gRPCdotnet.Workshop.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new WorkshopServices.WorkshopServicesClient(channel);
                //var workshops = client.AddAttendee(new AddAttendeeRequest());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();

        }
    }
}
