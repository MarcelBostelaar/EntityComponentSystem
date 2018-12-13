using System;
using System.Collections.Generic;
using TopologicalSort;

namespace Csharptestenvironment
{
    class Program
    {
        static void Main(string[] args)
        {
            var circularvalues = new List<Tuple<int, List<int>>>()
            {
                new Tuple<int, List<int>>(1, new List<int>(){2}),
                new Tuple<int, List<int>>(2, new List<int>(){1}),
                new Tuple<int, List<int>>(3, new List<int>(){1})
            };

            var valid = new List<Tuple<int, List<int>>>()
            {
                new Tuple<int, List<int>>(1, new List<int>()),
                new Tuple<int, List<int>>(2, new List<int>(){1}),
                new Tuple<int, List<int>>(3, new List<int>(){1}),
                new Tuple<int, List<int>>(4, new List<int>(){2}),
                new Tuple<int, List<int>>(5, new List<int>(){4}),
                new Tuple<int, List<int>>(6, new List<int>(){3}),
                new Tuple<int, List<int>>(7, new List<int>(){6,5})
            };

            var sorted = Functions.TopologicalSort(valid, x => x.Item1, x => x.Item2, (a, b) => a == b);

        }
    }
}
