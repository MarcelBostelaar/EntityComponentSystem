using System;
using System.Collections.Generic;
using System.Text;

namespace TopologicalSort
{
    class Node<ID, T>
    {
        public T value;
        public ID id;
        public List<Node<ID, T>> dependencies = new List<Node<ID, T>>();
        public int incoming_dependencies = 0;

        public Node(ID id, T value)
        {
            this.value = value;
            this.id = id;
        }

        public void addDependency(Node<ID,T> node)
        {
            this.dependencies.Add(node);
            node.incoming_dependencies++;
        }
    }
}
