using AutoMapper;
using GymBooking.Web.Models.Entities;
using GymBooking.Web.Models.ViewModels;

namespace GymBooking.Web.Data.AutoMapper
{
    public class GymClassProfile : Profile
    {
        public GymClassProfile()
        {
            //CreateMap<GymClass, GymClassesViewModel>();

            string id = null;
            CreateMap<GymClass, GymClassesViewModel>()
                    .ForMember(dest => dest.Attending,
                    from => from.MapFrom(g => g.AttendingMembers.Any(a => a.ApplicationUserId == id)));
            
            
        }
    }
}
