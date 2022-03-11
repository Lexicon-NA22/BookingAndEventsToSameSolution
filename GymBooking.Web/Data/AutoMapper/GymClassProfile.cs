using AutoMapper;
using GymBooking.Web.Models.Entities;
using GymBooking.Web.Models.ViewModels;

namespace GymBooking.Web.Data.AutoMapper
{
    public class GymClassProfile : Profile
    {
        public GymClassProfile()
        {
            CreateMap<GymClass, GymClassesViewModel>();
        }
    }
}
