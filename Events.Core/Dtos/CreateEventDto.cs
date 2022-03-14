using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Events.Core.Dtos
{
    public class CreateEventDto
    {
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public int Length { get; set; }


        public string LocationAddress { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
    }
}
