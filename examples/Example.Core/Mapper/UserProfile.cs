using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDLite.Mapper;

namespace Example.Core.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<Domain.User, Domain.User>()
                .IgnoreTrackProps()
                .ForMember(k => k.Password, opt => opt.Ignore())
                .ForMember(k => k.Salt, opt => opt.Ignore());
        }
    }
}
