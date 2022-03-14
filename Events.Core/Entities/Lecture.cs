using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Events.Core.Entities
{
    public class Lecture
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }

        public Guid CodeEventId { get; set; }
        public Guid? SpeakerId { get; set; }
        public CodeEvent CodeEvent { get; set; }
        public Speaker Speaker { get; set; }
    }
}
