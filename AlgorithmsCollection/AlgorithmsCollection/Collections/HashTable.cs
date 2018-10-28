using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Generic hash table with linked list separate chaining.
    /// </summary>
    /// <typeparam name="TKey">Element key type</typeparam>
    /// <typeparam name="TValue">Element value type</typeparam>
    public class HashTable<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        public const int TableSizeDefault = 1024;

        private IEqualityComparer<TKey> Comparer { get; set; } = null;
        private List<KeyValuePair<TKey, TValue>>[] table;

        /// <summary>
        /// Size of current table.
        /// </summary>
        public int TableSize => table.Length;

        /// <summary>
        /// Number of elements.
        /// </summary>
        public int Count { get; private set; } = 0;
        public bool IsReadOnly => false;
        
        /// <summary>
        /// List of all keys present in table.
        /// </summary>
        public List<TKey> Keys => table.SelectMany(list => list.Select(pair => pair.Key)).ToList();

        /// <summary>
        /// List of all present values in table.
        /// </summary>
        public List<TValue> Values => table.SelectMany(list => list.Select(pair => pair.Value)).ToList();
        
        /// <summary>
        /// Create hashtable with given table size (optional).
        /// </summary>
        /// <param name="tableSize">Size of the hashtable.</param>
        public HashTable(int tableSize = TableSizeDefault)
        {
            if (tableSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Table size must be positive");
            }
            table = new List<KeyValuePair<TKey, TValue>>[tableSize];
        }

        /// <summary>
        /// Create hashtable and fill it with elements from dictionary.
        /// </summary>
        /// <param name="dictionary">Elements to insert</param>
        /// <param name="tableSize">Size of the hashtable</param>
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

        /// <summary>
        /// Create hashtable and setup own equality comparer for hashcode computation.
        /// </summary>
        /// <param name="comparer">Comparer for computing hashcode</param>
        /// <param name="tableSize">Size of hashtable</param>
        public HashTable(IEqualityComparer<TKey> comparer, int tableSize = TableSizeDefault) : this(tableSize)
        {
            Comparer = comparer ?? throw new ArgumentNullException("Comparer is null");
        }

        /// <summary>
        /// Create hashtable and fill it with values from other hashtable.
        /// </summary>
        /// <param name="from">Hashtable to copy</param>
        public HashTable(HashTable<TKey, TValue> from) : this(from?.ToDictionary())
        {
        }

        /// <summary>
        /// Get/set value at given key.
        /// </summary>
        /// <param name="key">Element's key</param>
        /// <returns>Element's value</returns>
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

        /// <summary>
        /// Get value based on given key.
        /// </summary>
        /// <param name="key">Element's key</param>
        /// <returns>Element's value</returns>
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

        /// <summary>
        /// Add key-value pair into table.
        /// </summary>
        /// <param name="key">Element's key</param>
        /// <param name="value">Element's value</param>
        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        /// <summary>
        /// Add key-value pair into table.
        /// </summary>
        /// <param name="item">Key-value pair to add</param>
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

        /// <summary>
        /// Clear the hashtable and remove all elements.
        /// </summary>
        public void Clear()
        {
            var tableSize = TableSize;
            table = new List<KeyValuePair<TKey, TValue>>[tableSize];
            Count = 0;
        }
        
        /// <summary>
        /// Check whether hashtable contains an element with given key.
        /// </summary>
        /// <param name="key">Element's key to check</param>
        /// <returns>True if table contains element with given key, otherwise false</returns>
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

        /// <summary>
        /// Check whether hashtable contains given element (both key and value must match).
        /// </summary>
        /// <param name="item">Element to check</param>
        /// <returns>True if hashtable contains given element, otherwise false</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item) => table[ComputeHash(item.Key)].Contains(item);
        
        /// <summary>
        /// Remove first occurence of element with given key.
        /// </summary>
        /// <param name="key">Element's key</param>
        /// <returns>True if any element was removed, otherwise false</returns>
        public bool Remove(TKey key) => RemoveImpl(key, null);

        /// <summary>
        /// Remove first occurence of given element (both key and value must match).
        /// </summary>
        /// <param name="item">Element to remove</param>
        /// <returns>True if any element was removed, otherwise false</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item) => RemoveImpl(item.Key, item.Value);
        
        /// <summary>
        /// Set new tablesize and rehash all elements.
        /// </summary>
        /// <param name="newTableSize">Table's new size</param>
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
        
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("ArrayIndex is less than zero");
            }
            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Not enough space in array");
            }
            foreach (var pair in this)
            {
                array[arrayIndex++] = pair;
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
                foreach (var pair in this)
                {
                    if (!table.GetValue(pair.Key).Equals(pair.Value))
                    {
                        return false;
                    }
                }
                return true;
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
