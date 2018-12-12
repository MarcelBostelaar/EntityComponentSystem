using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TopologicalSort
{
    public static class Functions
    {
        private static void BuildTree<ID, T>(List<Tuple<List<ID>, Node<ID, T>>> nodes, Func<ID, ID, bool> equals)
        {
            foreach (var node in nodes)
            {
                foreach (var dependencyid in node.Item1)
                {
                    var dependencynode = nodes.Find(x => equals(x.Item2.id, dependencyid));
                    dependencynode.Item2.incoming_dependencies++;
                    node.Item2.dependencies.Add(dependencynode.Item2);
                }
            }
        }

        private static Tuple<List<Node<ID, T>>, List<Node<ID, T>>> KahnsAlgorithm<ID, T>(List<Node<ID, T>> nodes)
        {
            var nodependency = nodes.Where(x => x.incoming_dependencies == 0).ToList();
            var tosort = nodes.ToList();
            foreach (var item in nodependency)
            {
                tosort.Remove(item);
            }
            var sorted = new List<Node<ID, T>>();

            while (nodependency.Count > 0)
            {
                var selected = nodependency.First();
                nodependency.RemoveAt(0);
                sorted.Add(selected);
                foreach (var item in selected.dependencies)
                {
                    item.incoming_dependencies--;
                    if (item.incoming_dependencies == 0)
                    {
                        nodependency.Add(item);
                        tosort.Remove(item);
                    }
                }
            }
            return new Tuple<List<Node<ID, T>>, List<Node<ID, T>>>(sorted, tosort);
        }
    }
}
