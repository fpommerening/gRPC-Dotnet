namespace FP.gRPCdotnet.Workshop.Server.Business;

public class WorkshopRepository
{
    private readonly List<Workshop> _workshops =
    [
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
    ];

    public void AddAttendee(int id, string name, string firstname, string email)
    {
        var workshop = _workshops.FirstOrDefault(x => x.Id == id);
        if (workshop == null)
        {
            throw new IndexOutOfRangeException($"workshop with id {id} not exists");
        }
        workshop.Attendees.Add(new Person {Email = email, Name = name, FirstName = firstname});
    }

    public IReadOnlyCollection<Workshop> GetWorkshops()
    {
        return _workshops.ToList().AsReadOnly();
    }
}