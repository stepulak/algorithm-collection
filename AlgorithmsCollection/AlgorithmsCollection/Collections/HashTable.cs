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
        public int TableSize => table.Length;
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        
        public List<TKey> Keys => table.SelectMany(list => list.Select(pair => pair.Key)).ToList();
        public List<TValue> Values => table.SelectMany(list => list.Select(pair => pair.Value)).ToList();

        private IEqualityComparer<TKey> Comparer { get; set; } = null;
        private List<KeyValuePair<TKey, TValue>>[] table;

        public HashTable(int tableSize = TableSizeDefault)
        {
            if (tableSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Table size must be positive");
            }
            Clear();
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

        public TValue this[TKey key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                Add(key, value);
            }
        }

        public TValue GetValue(TKey key)
        {
            var list = table[ComputeHash(key)];
            var index = list != null ? FindIndexForKey(list, key) : -1;
            if (index < 0)
            {
                throw new ArgumentException("Item with given key not found");
            }
            return list[index].Value;
        }

        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            int hash = ComputeHash(item.Key);
            var list = table[hash] ?? (table[hash] = new List<KeyValuePair<TKey, TValue>>());
            var index = list.FindIndex(pair => pair.Equals(item));
            if (index >= 0)
            {
                list[index] = item; // Overwrite
            }
            else
            {
                list.Add(item);
                Count++;
            }
        }

        public void Clear()
        {
            var tableSize = TableSize;
            table = new List<KeyValuePair<TKey, TValue>>[tableSize];
            Count = 0;
        }

        public bool Contains(TKey key)
        {
            try
            {
                GetValue(key);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => table[ComputeHash(item.Key)].Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            var list = table[ComputeHash(key)];
            var index = list != null ? FindIndexForKey(list, key) : -1;
            if (index < 0)
            {
                return false;
            }
            list.RemoveAt(index);
            return true;
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var list in table)
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var pair in list)
                    {
                        yield return pair;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public void ChangeTableSizeAndRehash(int newTableSize)
        {

        }

        private int ComputeHash(TKey key) => (Comparer == null ? key.GetHashCode() : Comparer.GetHashCode(key)) % table.Length;
        private int FindIndexForKey(List<KeyValuePair<TKey, TValue>> list, TKey key) => list.FindIndex(pair => pair.Key.Equals(key));
    }
}
