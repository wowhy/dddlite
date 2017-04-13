using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDLite.Mapper;
using Example.Core.Domain;

namespace Example.Core.Mapper
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            this.CreateMap<Test, Test>()
                .IgnoreTrackProps();
        }
    }
}
