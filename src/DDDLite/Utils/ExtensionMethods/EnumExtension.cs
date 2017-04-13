namespace DDDLite.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class EnumExtension
    {
        private static ThreadLocal<Dictionary<Type, EnumInfo>> cache =
            new ThreadLocal<Dictionary<Type, EnumInfo>>(() => new Dictionary<Type, EnumInfo>());

        public static string GetDescription(this Enum @this)
        {
            var type = @this.GetType();
            var info = default(EnumInfo);

            if (!cache.Value.TryGetValue(type, out info))
            {
                cache.Value[type] = info = new EnumInfo(type);
            }

            return info.GetText(Convert.ToInt32(@this));
        }
    }
}