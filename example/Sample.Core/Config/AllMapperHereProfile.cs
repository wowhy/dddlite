namespace Sample.Core.Config
{
    using AutoMapper;

    using DDDLite.Config;

    using Sample.Core.Entity;
    using Sample.Core.DTO;

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
