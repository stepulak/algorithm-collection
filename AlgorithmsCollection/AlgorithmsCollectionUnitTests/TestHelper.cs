using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollectionUnitTests
{
    public static class TestHelper
    {
        public static bool IsSortedAscending<T>(IEnumerable<T> collection)
        {
            return Enumerable.SequenceEqual(collection, collection.OrderBy(a => a));
        }

        public static IComparer<T> DescedingComparer<T>()
        {
            return Comparer<T>.Create(new Comparison<T>((a, b) => (int)(dynamic)b - (dynamic)a));
        }
    }
}
