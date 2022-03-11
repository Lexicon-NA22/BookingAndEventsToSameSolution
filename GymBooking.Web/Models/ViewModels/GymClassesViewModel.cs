namespace GymBooking.Web.Models.ViewModels
#nullable disable
{
    public class GymClassesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Attending { get; set; }
    }
}
