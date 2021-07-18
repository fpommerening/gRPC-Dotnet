using System;
using System.Collections.Generic;

namespace FP.gRPCdotnet.Workshop.Server.Business
{
    public class Workshop
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Person Instructor { get; set; }

        public DateTime Date { get; set; }

        public float Price { get; set; }

        public Level Level { get;set; }

        public List<Person> Attendees { get; } = new List<Person>();
    }
}
