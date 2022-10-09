using System.Linq.Expressions;
using System.Reflection;

namespace CommonTools
{
    public static class PropertyCopyrighter<T>
    {
        private static readonly Action<T, T> copier;

        static PropertyCopyrighter()
        {
            var p1 = Expression.Parameter(typeof(T), "from");
            var p2 = Expression.Parameter(typeof(T), "to");

            var props = from property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        where property.CanRead && property.CanWrite
                        select Expression.Assign(Expression.Property(p2, property), Expression.Property(p1, property));

            copier = Expression.Lambda<Action<T, T>>(Expression.Block(props), p1, p2).Compile();
        }

        public static void CopyAllProperties(T from, T to) => copier(from, to);
    }
}
