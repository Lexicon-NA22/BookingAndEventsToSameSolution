using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Events.Core.Entities
{
    public class CodeEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public int Length { get; set; }

        public Location Location { get; set; }

        public ICollection<Lecture> Lectures { get; set; }


    }
}
