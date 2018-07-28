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
        public int Count { get; private set; } = 0;
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

        public HashTable(HashTable<TKey, TValue> from) : this(from?.ToDictionary())
        {
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
            var index = FindIndexForKey(list, item.Key);
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

        public bool Remove(TKey key) => RemoveImpl(key, null);
        public bool Remove(KeyValuePair<TKey, TValue> item) => RemoveImpl(item.Key, item.Value);
        
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
            if (newTableSize <= 0)
            {
                throw new ArgumentException("Table size must be positive!");
            }
            var oldTable = table;
            Count = 0;
            table = new List<KeyValuePair<TKey, TValue>>[newTableSize];
            foreach (var list in oldTable)
            {
                list?.ForEach(pair => Add(pair));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is HashTable<TKey, TValue> table)
            {
                if (table.Count != Count)
                {
                    return false;
                }
                return table.ToDictionary().Equals(ToDictionary());
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            int result = 0;
            foreach (var pair in this)
            {
                result ^= pair.Key.GetHashCode();
                result ^= pair.Value.GetHashCode();
            }
            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var pair in ToDictionary()) // In case of different order
            {
                builder.Append($"{pair.Key}: {pair.Value};");
            }
            return builder.ToString();
        }

        public Dictionary<TKey, TValue> ToDictionary() => this.ToDictionary(p => p.Key, p => p.Value);
        
        private int ComputeHash(TKey key) => Math.Abs((Comparer == null ? key.GetHashCode() : Comparer.GetHashCode(key))) % table.Length;
        private int FindIndexForKey(List<KeyValuePair<TKey, TValue>> list, TKey key) => list.FindIndex(pair => pair.Key.Equals(key));

        private bool RemoveImpl(TKey key, Optional<TValue> value)
        {
            var list = table[ComputeHash(key)];
            if (list == null)
            {
                return false;
            }
            int index;
            if (value != null && value.HasValue)
            {
                var keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
                index = list.FindIndex(pair => pair.Equals(keyValuePair));
            }
            else
            {
                index = FindIndexForKey(list, key);
            }
            if (index < 0)
            {
                return false;
            }
            list.RemoveAt(index);
            Count--;
            return true;
        }
    }
}
