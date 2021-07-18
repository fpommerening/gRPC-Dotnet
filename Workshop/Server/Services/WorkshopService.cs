using System;
using System.Linq;
using System.Threading.Tasks;
using FP.gRPCdoetnet.Workshop.Contract;
using FP.gRPCdotnet.Workshop.Server.Business;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace FP.gRPCdotnet.Workshop.Server.Services
{
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
                var workhop = new gRPCdoetnet.Workshop.Contract.Workshop
                {
                    Name = repoWorkshop.Name,
                    Instructor = MapPerson(repoWorkshop.Instructor),
                    Id = repoWorkshop.Id,
                    Level = (gRPCdoetnet.Workshop.Contract.Workshop.Types.Level)(int)repoWorkshop.Level,
                    Price = repoWorkshop.Price,
                    Date = Timestamp.FromDateTime(repoWorkshop.Date),
                };
                workhop.Attendees.AddRange(repoWorkshop.Attendees.Select(MapPerson));

                response.Items.Add(workhop);
            }

            return Task.FromResult(response);
        }
     
        private gRPCdoetnet.Workshop.Contract.Workshop.Types.Person MapPerson(Business.Person person)
        {
            return new gRPCdoetnet.Workshop.Contract.Workshop.Types.Person
            {
                Name = person.Name,
                Firstname = person.FirstName,
                Email = person.Email,
            };
        }
    }
}
