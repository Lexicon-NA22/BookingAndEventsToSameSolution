namespace GymBooking.Web.Models.ViewModels
#nullable disable

{
    public class IndexViewModel
    {
        public IEnumerable<GymClassesViewModel> GymClasses { get; set; }
        public bool ShowHistory { get; set; }
    }
}
