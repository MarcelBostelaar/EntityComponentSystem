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

        private static IEnumerable<Node<ID, T>> RemoveOnceWithNoDependencies<ID, T>(this IEnumerable<Node<ID, T>> nodes)
        {
            var with_no_outgoingdependencies = nodes.Where(x => x.dependencies.Count == 0).Evaluate();
            var with_outgoingdependencies = nodes.Where(x => x.dependencies.Count > 0).Evaluate(); //lazy evalutation causes odd behaviour (double excecution of this filter) so it is evaluated to ensure it does what the function title says, although "OnlyWithDependencies" wont change behaviour. If no changes are made to "OnlyWithDependencies" after 14-feb-2019, and it is not used anywhere else, it could be removed to gain slightly more performance.
            foreach (var toremove in with_no_outgoingdependencies)
            {
                foreach (var removefrom in with_outgoingdependencies)
                {
                    removefrom.dependencies.Remove(toremove);
                }
            }
            return with_outgoingdependencies;
        }

        public static IEnumerable<Node<ID, T>> OnlyWithDependencies<ID,T>(this IEnumerable<Node<ID,T>> nodes)
        {
            var reduced = RemoveOnceWithNoDependencies(nodes);
            while (nodes.Count() != reduced.Count())
            {
                nodes = reduced;
                reduced = RemoveOnceWithNoDependencies(nodes);
            }
            return reduced;
        }
    }
}
