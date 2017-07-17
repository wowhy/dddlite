namespace DDDLite.ExtensionMethods
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class MapperExtension
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
