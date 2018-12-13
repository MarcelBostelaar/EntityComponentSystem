using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TopologicalSort
{
    public static class Functions
    {


        public static Tuple<IEnumerable<Node<ID, T>>, IEnumerable<Node<ID, T>>> TopologicalSort<T,ID>(IEnumerable<T> values, Func<T, ID> id_grabber, Func<T, IEnumerable<ID>> dependency_grabber, Func<ID,ID,bool> ID_equality)
        {
            var allids = values.Select(id_grabber);
            var alldependencies = values.Select(dependency_grabber).SelectMany(x => x);
            if (! allids.All(single => allids.Where(x => ID_equality(single, x)).Count() == 1))
            {
                throw new Exception("Ids are not unique");
            }
            if(! alldependencies.All(x => allids.Where(y => ID_equality(x,y)).Count() == 1))
            {
                throw new Exception("Some dependencies do not exist as an ID in the values");
            }
            var halfnodes = BuildNodesNoDependencies(values, id_grabber, dependency_grabber);
            AddDependencies(halfnodes, ID_equality);
            var nodes = halfnodes.Select(x => x.Item2);
            return KahnsAlgorithm(nodes);
        }

        private static IEnumerable<Tuple<IEnumerable<ID>, Node<ID, T>>> BuildNodesNoDependencies<ID, T>(IEnumerable<T> values, Func<T, ID> id_grabber, Func<T, IEnumerable<ID>> dependency_grabber)
        {
            var nodes = values.Select(x => new Node<ID,T>(id_grabber(x), x)).ToList();
            return nodes.Select(x => new Tuple<IEnumerable<ID>, Node<ID, T>>(dependency_grabber(x.value), x)).ToList();
        }

        private static void AddDependencies<ID, T>(IEnumerable<Tuple<IEnumerable<ID>, Node<ID, T>>> halfnodes, Func<ID, ID, bool> equals)
        {
            foreach (var node in halfnodes)
            {
                foreach (var dependencyid in node.Item1)
                {
                    var dependencynode = halfnodes.Where(x => equals(x.Item2.id, dependencyid)).First();
                    dependencynode.Item2.incoming_dependencies++;
                    node.Item2.dependencies.Add(dependencynode.Item2);
                }
            }
        }

        private static Tuple<IEnumerable<Node<ID, T>>, IEnumerable<Node<ID, T>>> KahnsAlgorithm<ID, T>(IEnumerable<Node<ID, T>> nodes)
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
            return new Tuple<IEnumerable<Node<ID, T>>, IEnumerable<Node<ID, T>>>(sorted, tosort);
        }
    }
}
