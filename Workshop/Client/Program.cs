﻿using FP.gRPCdotnet.Workshop.Contract;
using Grpc.Net.Client;

try
{
    var channel = GrpcChannel.ForAddress("http://localhost:5002");
    var client = new WorkshopServices.WorkshopServicesClient(channel);
    var workshops = await client.GetWorkshopsAsync(new GetWorkshopsRequest());
    // client.AddAttendee(new AddAttendeeRequest
    // {
    //     Person = new Person
    //     {
    //         Name = "Mustermann",
    //         Firstname = "Sabine",
    //         Email = "smustermann@demo-apps.de"
    //     },
    //     Id = 42
    // }, new Metadata());
}
catch(Exception ex)
{
    Console.WriteLine(ex);
}

Console.ReadLine();


