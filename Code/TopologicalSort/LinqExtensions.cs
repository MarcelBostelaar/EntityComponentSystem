using System;
using System.Collections.Generic;
using System.Linq;

namespace TopologicalSort
{
    static class LinqExtensions
    {
        public static bool Distinct<T>(this IEnumerable<T> values, Func<T,T,bool> equality_function)
        {
            return values.All(x => values.ContainsCount(x, equality_function) == 1);
        }

        public static int ContainsCount<T>(this IEnumerable<T> values, T value, Func<T, T, bool> equality_function)
        {
            return values.Where(x => equality_function(x, value)).Count();
        }

        public static bool Contains<T>(this IEnumerable<T> values, T value, Func<T, T, bool> equality_function)
        {
            return values.ContainsCount(value, equality_function) > 0;
        }

        public static IEnumerable<T> Evaluate<T>(this IEnumerable<T> values)
        {
            return values.ToArray();
        }

        public static IEnumerable<Node<ID, T>> OnlyWithDependencies<ID,T>(this IEnumerable<Node<ID,T>> nodes)
        {
            return nodes.Where(x => x.dependencies.Count > 0);
        }
    }
}
