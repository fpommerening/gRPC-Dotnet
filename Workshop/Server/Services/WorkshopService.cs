using FP.gRPCdotnet.Workshop.Contract;
using FP.gRPCdotnet.Workshop.Server.Business;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace FP.gRPCdotnet.Workshop.Server.Services;

public class WorkshopService : WorkshopServices.WorkshopServicesBase
{
    private readonly ILogger<WorkshopService> _logger;
    private readonly WorkshopRepository _repository;

    public WorkshopService(ILogger<WorkshopService> logger, WorkshopRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

   
}