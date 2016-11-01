namespace Sample.Core.Mappers
{
    using AutoMapper;

    using DDDLite.Config;

    using Domain;
    using DTO;

    public class AllMapperHereProfile : Profile
    {
        public AllMapperHereProfile()
        {
            this.CreateMap<Blog, Blog>()
                .UseAggregateRootMap();

            this.CreateMap<Blog, BlogDTO>();

            this.CreateMap<Post, Post>()
                .UseAggregateRootMap();

            this.CreateMap<Post, PostDTO>();
        }
    }
}
