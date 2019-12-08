#region Usings

using System.Collections.Generic;

#endregion


namespace Eshva.DockerCompose.Extensions
{
    public static class ListExtensions
    {
        public static List<TValue> AddConditionally<TValue>(this List<TValue> list, bool shouldAdd, TValue value)
        {
            if (shouldAdd)
            {
                list.Add(value);
            }

            return list;
        }
    }
}
