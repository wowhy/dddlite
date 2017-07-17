namespace DDDLite.ExtensionMethods
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class MapperExtension
    {
        //public static IMappingExpression<TSource, TDestination> IgnoreTrackProps<TSource, TDestination>(
        //    this IMappingExpression<TSource, TDestination> @this)
        //    where TSource : IAggregateRoot
        //    where TDestination : IAggregateRoot
        //{
        //    return @this.ForMember(k => k.RowVersion, opt => opt.Ignore())
        //       .ForMember(k => k.CreatedById, opt => opt.Ignore())
        //       .ForMember(k => k.CreatedOn, opt => opt.Ignore())
        //       .ForMember(k => k.ModifiedById, opt => opt.Ignore())
        //       .ForMember(k => k.ModifiedOn, opt => opt.Ignore());
        //}
    }
}
