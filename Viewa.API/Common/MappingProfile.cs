using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viewa.Db;
using Viewa.Models;

namespace Viewa.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<SampleData, EventItem>();
            CreateMap<Users, UserData>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username));
        }
    }
}
