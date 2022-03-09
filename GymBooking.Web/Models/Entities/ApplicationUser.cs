using Microsoft.AspNetCore.Identity;

namespace GymBooking.Web.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
