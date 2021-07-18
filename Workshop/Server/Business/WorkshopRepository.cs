using System;
using System.Collections.Generic;
using System.Linq;

namespace FP.gRPCdotnet.Workshop.Server.Business
{
    public class WorkshopRepository
    {
        private List<Workshop> workshops = new List<Workshop>
        {
            new Workshop
            {
                Id = 42,
                Name = "Kubernetes für Entwickler",
                Date = new DateTime(2021, 08, 09, 07, 00, 00, DateTimeKind.Utc),
                Instructor = new Person
                {
                    FirstName = "Frank",
                    Name = "Pommerening",
                    Email = "fpommerening@demo-apps.de"
                },
                Price = 799,
                Level = Level.Beginner,
                Attendees =
                {
                    new Person
                    {
                        Name = "Mustermann",
                        FirstName = "Max",
                        Email = "mmustermann@demo-apps.de"
                    }
                }
            },
            new Workshop
            {
                Id = 88,
                Name = "Kubernetes für DevOps",
                Date = new DateTime(2021, 10, 15, 07, 00, 00, DateTimeKind.Utc),
                Instructor = new Person
                {
                    FirstName = "Frank",
                    Name = "Pommerening",
                    Email = "fpommerening@demo-apps.de"
                },
                Price = 877,
                Level = Level.Advanced,
                Attendees =
                {
                    new Person
                    {
                        Name = "Mustermann",
                        FirstName = "Max",
                        Email = "mmustermann@demo-apps.de"
                    }
                }
            }
        };

        public void AddAttendee(int id, string name, string firstname, string email)
        {
            var workhop = workshops.FirstOrDefault(x => x.Id == id);
            if (workhop == null)
            {
                throw new IndexOutOfRangeException($"workshop with id {id} not exists");
            }
            workhop?.Attendees.Add(new Person {Email = email, Name = name, FirstName = firstname});
        }

        public IReadOnlyCollection<Workshop> GetWorkshops()
        {
            return workshops.ToList().AsReadOnly();
        }
    }
}
