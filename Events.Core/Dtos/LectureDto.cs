using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Core.Dtos
{
    public class LectureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Level { get; set; }

        public Guid SpeakerId { get; set; }
    }
}
