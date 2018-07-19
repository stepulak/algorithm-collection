using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class HashTable<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        public const int TableSizeDefault = 1024;
        public int TableSize { get; private set; } = 1024;
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        
        public List<TKey> Keys => throw new NotImplementedException();
        public List<TValue> Values => throw new NotImplementedException();

        private IEqualityComparer<TKey> Comparer { get; set; } = null;
        private List<KeyValuePair<TKey, TValue>>[] table;

        public HashTable(int tableSize = TableSizeDefault)
        {
            if (tableSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Table size must be positive");
            }
            table = new List<KeyValuePair<TKey, TValue>>[tableSize];
        }

        public HashTable(IDictionary<TKey, TValue> dictionary, int tableSize = TableSizeDefault) : this(tableSize)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("Dictionary is null");
            }
            foreach (var pair in dictionary)
            {
                Add(pair);
            }
        }

        public HashTable(IEqualityComparer<TKey> comparer, int tableSize = TableSizeDefault) : this(tableSize)
        {
            Comparer = comparer ?? throw new ArgumentNullException("Comparer is null");
        }
        
        public void Add(TKey key, TValue value)
        {

        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        
        public void ChangeTableSizeAndRehash(int newTableSize)
        {

        }

        private int ComputeHash(TKey key) => Comparer == null ? key.GetHashCode() : Comparer.GetHashCode(key);
    }
}
