namespace DDDLite.Domain
{
    using AutoMapper;

    public static class MapperHelper
    {
        public static IMapper GetOrCreateMapper<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot
        {
            var mapper = default(IMapper);
            if (Mapper.Configuration.FindTypeMapFor<TAggregateRoot, TAggregateRoot>() != null)
            {
                mapper = Mapper.Instance;
            }
            else
            {
                IConfigurationProvider config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TAggregateRoot, TAggregateRoot>()
                       .ForMember(k => k.Id, k => k.Ignore())
                       .ForMember(k => k.CreatedById, k => k.Ignore())
                       .ForMember(k => k.CreatedOn, k => k.Ignore())
                       .ForMember(k => k.ModifiedById, k => k.Ignore())
                       .ForMember(k => k.ModifiedOn, k => k.Ignore())
                       .ForMember(k => k.RowVersion, k => k.Ignore());
                });
                mapper = new Mapper(config);
            }

            return mapper;
        }
    }
}