using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public static class Sort
    {
        /// <summary>
        /// Standard C-like in-place quicksort implementation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="comparer"></param>
        public static void QuickSortClassic<T>(this T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("Comparer is null");
            }
            QuickSortClassicImpl(array, 0, array.Length, comparer);
        }

        /// <summary>
        /// Haskell-like quicksort implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> QuickSortLinq<T>(this IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("Comparer is null");
            }
            return QuickSortLinqImpl(collection, comparer);
        }
        
        public static void HeapSort<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("Comparer is null");
            }
            var heap = new BinaryHeap<T>(comparer, array);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = heap.Pop();
            }
        }

        public static void MergeSort<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("Comparer is null");
            }
            if (array.Length > 1)
            {
                var copy = array.ToArray();
                MergeSortImpl(array, copy, 0, array.Length - 1, comparer);
            }
        }

        private static void MergeSortImpl<T>(T[] source, T[] destination, int left, int right, IComparer<T> comparer)
        {
            var middle = (left + right) / 2;
            if (left < middle)
            {
                MergeSortImpl(source, destination, left, middle, comparer);
            }
            if (middle < right)
            {
                MergeSortImpl(source, destination, middle + 1, right, comparer);
            }
            Merge(source, destination, left, middle, right, comparer);
            Array.Copy(destination, left, source, left, right - left + 1);
        }

        private static void Merge<T>(T[] source, T[] destination, int left, int middle, int right, IComparer<T> comparer)
        {
            int leftIndex = left;
            int rightIndex = middle + 1;
            int destinationIndex = left;
            while (leftIndex <= middle && rightIndex <= right)
            {
                if (comparer.Compare(source[leftIndex], source[rightIndex]) < 0)
                {
                    destination[destinationIndex] = source[leftIndex++];
                }
                else
                {
                    destination[destinationIndex] = source[rightIndex++];
                }
                destinationIndex++;
            }
            // Copy the rest
            if (leftIndex <= middle)
            {
                Array.Copy(source, leftIndex, destination, destinationIndex, middle - leftIndex + 1);
            }
            else if (rightIndex <= right)
            {
                Array.Copy(source, rightIndex, destination, destinationIndex, right - rightIndex + 1);
            }
        }
        
        private static void QuickSortClassicImpl<T>(T[] array, int left, int right, IComparer<T> comparer)
        {
            if (left < right)
            {
                int pivot = QuickSortClassicPartition(array, left, right, comparer);
                QuickSortClassicImpl(array, left, pivot, comparer);
                QuickSortClassicImpl(array, pivot + 1, right, comparer);
            }
        }

        private static int QuickSortClassicPartition<T>(T[] array, int left, int right, IComparer<T> comparer)
        {
            int pivot = left;
            while (true)
            { 
                do
                {
                    left++;
                } while (left < right && comparer.Compare(array[left], array[pivot]) <= 0);
                do
                {
                    right--;
                } while (comparer.Compare(array[right], array[pivot]) > 0);

                if (left >= right)
                {
                    break;
                }
                Swap(ref array[left], ref array[right]);
            }
            Swap(ref array[pivot], ref array[right]);
            return right;
        }

        private static IEnumerable<T> QuickSortLinqImpl<T>(this IEnumerable<T> collection, IComparer<T> comparer)
        {
            var length = collection.Count();
            if (length == 0)
            {
                return Enumerable.Empty<T>();
            }
            if (length == 1)
            {
                return collection.ToList();
            }
            var pivot = collection.First();
            return QuickSortLinq(collection.Skip(1).Where(e => comparer.Compare(e, pivot) <= 0), comparer)
                .Concat(new T[] { pivot })
                .Concat(QuickSortLinq(collection.Where(e => comparer.Compare(e, pivot) > 0), comparer));
        }

        private static void Swap<T>(ref T v1, ref T v2)
        {
            T temp = v1;
            v1 = v2;
            v2 = temp;
        }
    }
}
