namespace DDDLite.ExtensionMethods
{
    using System;

    public static class FlagExtension
    {
        public static bool IsSet<TEnum>(this TEnum flags, TEnum flagToTest)
            where TEnum : struct
        {
            var value = Convert.ToInt64(flags);
            var valueToTest = Convert.ToInt64(flagToTest);

            return (value & valueToTest) == valueToTest;
        }

        public static bool IsSet<TEnum>(this Nullable<TEnum> flags, TEnum flagToTest)
            where TEnum : struct
        {
            if (flags == null)
                return false;

            return IsSet(flags.Value, flagToTest);
        }

        public static bool IsClear<TEnum>(this TEnum flags, TEnum flagToTest)
            where TEnum : struct
        {
            return !IsSet(flags, flagToTest);
        }

        public static bool IsClear<TEnum>(this Nullable<TEnum> flags, TEnum flagToTest)
            where TEnum : struct
        {
            if (flags == null)
                return false;
            return !IsSet(flags.Value, flagToTest);
        }

        public static bool AnyFlagsSet<TEnum>(this TEnum flags, TEnum testFlags)
            where TEnum : struct
        {
            var value = Convert.ToInt64(flags);
            var testValues = Convert.ToInt64(flags);

            return (value & testValues) != 0;
        }

        public static bool AnyFlagsSet<TEnum>(this Nullable<TEnum> flags, TEnum testFlags)
            where TEnum : struct
        {
            if (flags == null)
                return false;

            return AnyFlagsSet(flags.Value, testFlags);
        }
    }
}