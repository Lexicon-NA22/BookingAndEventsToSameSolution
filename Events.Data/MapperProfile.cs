using AutoMapper;
using Events.Core.Dtos;
using Events.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CodeEvent, CodeEventDto>().ReverseMap();
            CreateMap<Lecture, LectureDto>().ReverseMap();
            CreateMap<Lecture, LectureCreateDto>().ReverseMap();
            CreateMap<CodeEvent, CreateEventDto>().ReverseMap();
        }
    }
}
