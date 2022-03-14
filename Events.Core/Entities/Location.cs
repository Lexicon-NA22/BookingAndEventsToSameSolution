using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Events.Core.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string CityTown { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public CodeEvent CodeEvent { get; set; }
        public Guid CodeEventId { get; set; }
    }
}
