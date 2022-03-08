using Microsoft.AspNetCore.Identity;

namespace GymBooking.Web.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
