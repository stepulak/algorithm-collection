using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class HashTableTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HashTableNegativeTableSize()
        {
            new HashTable<string, int>(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashTableNullDictionary()
        {
            Dictionary<int, int> dic = null;
            new HashTable<int, int>(dic);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashTableNullComparer()
        {
            IEqualityComparer<int> cmp = null;
            new HashTable<int, int>(cmp);
        }

        [TestMethod]
        public void HashTableSpecificTableSize()
        {
            var table = new HashTable<int, double>(100);
            Assert.AreEqual(table.TableSize, 100);
        }

        [TestMethod]
        public void HashTableAddAndGetSimple()
        {
            var table = new HashTable<string, int>();
            table.Add("one", 1);
            table.Add(new KeyValuePair<string, int>("two", 2));
            Assert.IsTrue(table.Contains("one"));
            Assert.IsTrue(table.Contains("two"));
            Assert.AreEqual(table.GetValue("one"), 1);
            Assert.AreEqual(table.GetValue("two"), 2);
            Assert.AreEqual(table.Count, 2);
        }

        [TestMethod]
        public void HashTableAddAndGetAdvanced()
        {
            var table = new HashTable<string, string>
            {
                { "one", "1" },
                { "two", "2" },
                { "three", "3" }
            };
            Assert.AreEqual(table.GetValue("one"), "1");
            Assert.AreEqual(table.GetValue("two"), "2");
            Assert.AreEqual(table.GetValue("three"), "3");
            table.Add("one", "11");
            Assert.AreEqual(table.GetValue("one"), "11");
            Assert.AreEqual(table.Count, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HashTableGetValueNotFound()
        {
            var table = new HashTable<int, int>()
            {
                { 1, 2 },
                { 2, 3 }
            };
            Assert.IsFalse(table.Contains(3));
            table.GetValue(table.GetValue(3));
        }

        [TestMethod]
        public void HashTableIndexer()
        {
            var table = new HashTable<string, bool>();
            table["hello"] = true;
            table["world"] = false;
            Assert.IsTrue(table["hello"]);
            Assert.IsFalse(table["world"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HashTableIndexerNotFound()
        {
            var table = new HashTable<string, bool>()
            {
                { "hello", true },
                { "world", false }
            };
            var value = table["not_found"];
        }

        [TestMethod]
        public void HashTableClear()
        {
            var table = new HashTable<string, int>()
            {
                { "a", 1 },
                { "b", 2 },
                { "c", 3 },
                { "d", 4 },
            };
            Assert.AreEqual(table.Count, 4);
            table.Clear();
            Assert.AreEqual(table.Count, 0);
            Assert.IsFalse(table.Contains("a"));
            Assert.IsFalse(table.Contains("b"));
            Assert.IsFalse(table.Contains("c"));
            Assert.IsFalse(table.Contains("d"));
        }

        [TestMethod]
        public void HashTableContains()
        {
            var table = new HashTable<string, int>()
            {
                { "a", 1 },
                { "b", 2 },
                { "c", 3 },
                { "d", 4 },
            };
            Assert.IsTrue(table.Contains("a"));
            Assert.IsFalse(table.Contains("e"));
            Assert.IsTrue(table.Contains(new KeyValuePair<string, int>("c", 3)));
            Assert.IsFalse(table.Contains(new KeyValuePair<string, int>("c", 4)));

        }

        [TestMethod]
        public void HashTableRemove()
        {
            var table = new HashTable<char, int>()
            {
                { 'a', 1 },
                { 'b', 2 },
                { 'c', 3 }
            };
            Assert.IsTrue(table.Remove('a'));
            Assert.IsFalse(table.Contains('a'));
            table.Remove(new KeyValuePair<char, int>('b', 2));
            Assert.IsFalse(table.Contains('b'));
            table.Remove(new KeyValuePair<char, int>('c', 4));
            Assert.IsTrue(table.Contains('c'));
            Assert.AreEqual(table.Count, 1);
        }

        [TestMethod]
        public void HashTableEnumerator()
        {
            var table = new HashTable<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 }
            };
            var dictionary = new Dictionary<KeyValuePair<string, int>, int>()
            {
                { new KeyValuePair<string, int>("one", 1), 0 },
                { new KeyValuePair<string, int>("two", 2), 0 },
                { new KeyValuePair<string, int>("three", 3), 0 },
                { new KeyValuePair<string, int>("four", 4), 0 },
                { new KeyValuePair<string, int>("five", 5), 0 },
            };
            foreach (var pair in table)
            {
                dictionary[pair] = dictionary[pair] + 1;
            }
            foreach (var pair in dictionary)
            {
                Assert.AreEqual(pair.Value, 1);
            }
        }

        [TestMethod]
        public void HashTableChangeTableSize()
        {
            var table = new HashTable<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 }
            };
            var newTableSize = table.TableSize / 20;
            table.ChangeTableSizeAndRehash(newTableSize);
            Assert.AreEqual(table.GetValue("one"), 1);
            Assert.AreEqual(table.GetValue("two"), 2);
            Assert.AreEqual(table.GetValue("three"), 3);
            Assert.AreEqual(table.GetValue("four"), 4);
            Assert.AreEqual(table.GetValue("five"), 5);
            Assert.AreEqual(table.Count, 5);
            Assert.AreEqual(table.TableSize, newTableSize);
        }

        [TestMethod]
        public void HashTableEquals()
        {
            var table1 = new HashTable<int, int>();
            var table2 = new HashTable<string, int>()
            {
                { "one", 1 }
            };
            HashTable<int, int> table3 = null;
            var table4 = new HashTable<string, int>()
            {
                { "one", 1 }
            };
            Assert.IsFalse(table1.Equals(table2));
            Assert.IsFalse(table1.Equals(table3));
            Assert.IsFalse(table1.Equals(null));
            Assert.IsFalse(table2.Equals(table3));
            Assert.IsTrue(table2.Equals(table4));
            Assert.IsTrue(table2.Equals(table2));
            Assert.IsTrue(table4.Equals(table4));
        }

        [TestMethod]
        public void HashTableConstructorFromHashTable()
        {
            var table1 = new HashTable<int, int>()
            {
                { 1, 2 },
                { 2, 3 },
                { 3, 4 }
            };
            var table2 = new HashTable<int, int>(table1);
            Assert.AreEqual(table2.Count, 3);
            Assert.AreEqual(table2[1], 2);
            Assert.AreEqual(table2[2], 3);
            Assert.AreEqual(table2[3], 4);
        }

        [TestMethod]
        public void HashTableHashCode()
        {
            var table1 = new HashTable<string, string>()
            {
                { "hello", "world" },
                { "hello", "kitty" }
            };
            var table2 = new HashTable<string, string>()
            {
                { "one", "two" }
            };
            var table3 = new HashTable<string, string>()
            {
                { "one", "two" }
            };
            Assert.AreNotEqual(table1.GetHashCode(), table2.GetHashCode());
            Assert.AreEqual(table2.GetHashCode(), table3.GetHashCode());
        }

        [TestMethod]
        public void HashTableString()
        {
            var table = new HashTable<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
            };
            Assert.AreEqual(table.ToString(), "one: 1;two: 2;");
        }

        [TestMethod]
        public void HashTableCopyTo()
        {
            var values = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("one", 1),
                new KeyValuePair<string, int>("two", 2),
                new KeyValuePair<string, int>("three", 3),
            };
            var table = new HashTable<string, int>();
            foreach (var value in values)
            {
                table.Add(value);
            }
            var array = new KeyValuePair<string, int>[3];
            table.CopyTo(array, 0);
            Assert.IsTrue(array.SequenceEqual(values));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HashTableCopyToNoSpaceLeft()
        {
            var table = new HashTable<int, int>()
            {
                { 1, 1 },
                { 2, 2 }
            };
            var array = new KeyValuePair<int, int>[3];
            table.CopyTo(array, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HashTableCopyToNegativeIndex()
        {
            new HashTable<int, int>().CopyTo(new KeyValuePair<int, int>[1], -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashTableCopyToNullArray()
        {
            new HashTable<int, int>().CopyTo(null, 0);
        }
    }
}
