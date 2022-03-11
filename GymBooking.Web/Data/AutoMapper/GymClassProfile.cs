using AutoMapper;
using GymBooking.Web.Models.Entities;
using GymBooking.Web.Models.ViewModels;
using System.Security.Claims;

namespace GymBooking.Web.Data.AutoMapper
{
    public class GymClassProfile : Profile
    {
        public GymClassProfile()
        {
            //CreateMap<GymClass, GymClassesViewModel>();

            //string id = null;
            //CreateMap<GymClass, GymClassesViewModel>()
            //        .ForMember(dest => dest.Attending,
            //        from => from.MapFrom(g => g.AttendingMembers.Any(a => a.ApplicationUserId == id)));  


            //CreateMap<GymClass, GymClassesViewModel>()
            //        .ForMember(dest => dest.Attending,
            //        from => from.MapFrom((src, dest, _, context)
            //        => src.AttendingMembers.Any(a => a.ApplicationUserId == context.Items["id"].ToString())));


            CreateMap<GymClass, GymClassesViewModel>()
                    .ForMember(dest => dest.Attending,
                    from => from.MapFrom<AttendingResolver>());

        }

    }

    public class AttendingResolver : IValueResolver<GymClass, GymClassesViewModel, bool>
    {
        private readonly IHttpContextAccessor http;

        public AttendingResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.http = httpContextAccessor;
        }

        public bool Resolve(GymClass source, GymClassesViewModel destination, bool destMember, ResolutionContext context)
        {
            return source.AttendingMembers is null ? false :
                        source.AttendingMembers.Any(a => a.ApplicationUserId == http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
