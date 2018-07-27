using System;
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
            /*Assert.IsTrue(table.Remove('a'));
            Assert.IsFalse(table.Contains('a'));
            table.Remove()*/
        }
    }
}
