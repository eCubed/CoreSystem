using System;

/// <summary>
/// Summary description for EnumTools
/// </summary>
namespace FCore.Foundations
{
    public static class EnumTools
    {
        public static T GetEnumInstance<T>(int v) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), v);
        }

        public static U GetValue<T, U>(T enumInstance) where T : struct, IConvertible
        {
            return (U)(object)enumInstance;
        }
    }
}