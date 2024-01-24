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

    public override Task<GetWorkshopsResponse> GetWorkshops(GetWorkshopsRequest request, ServerCallContext context)
    {

        var response = new GetWorkshopsResponse();
        foreach (var repoWorkshop in _repository.GetWorkshops())
        {
            var workhop = new gRPCdotnet.Workshop.Contract.Workshop
            {
                Name = repoWorkshop.Name,
                Instructor = MapPerson(repoWorkshop.Instructor),
                Id = repoWorkshop.Id,
                Level = (gRPCdotnet.Workshop.Contract.Workshop.Types.Level)(int)repoWorkshop.Level,
                Price = repoWorkshop.Price,
                Date = Timestamp.FromDateTime(repoWorkshop.Date),
            };
            workhop.Attendees.AddRange(repoWorkshop.Attendees.Select(MapPerson));

            response.Items.Add(workhop);
        }
        return Task.FromResult(response);
    }

    public override Task<Empty> AddAttendee(AddAttendeeRequest request, ServerCallContext context)
    {
        try
        {
            _repository.AddAttendee(request.Id, request.Person.Name, request.Person.Firstname,
                request.Person.Email);
        }
        catch (IndexOutOfRangeException iex)
        {
            context.Status = new Status(StatusCode.InvalidArgument, iex.Message);
        }
            
        return Task.FromResult(new Empty());
    }

    private Contract.Person MapPerson(Business.Person person)
    {
        return new Contract.Person
        {
            Name = person.Name,
            Firstname = person.FirstName,
            Email = person.Email,
        };
    }
}