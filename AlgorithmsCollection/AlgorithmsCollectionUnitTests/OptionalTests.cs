using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgorithmsCollection;

namespace AlgorithmsCollectionUnitTests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void OptionalConstructorEmpty()
        {
            var opt = new Optional<bool>();
            Assert.IsFalse(opt.HasValue);
        }

        [TestMethod]
        public void OptionalConstructorValue()
        {
            var opt = new Optional<bool>(true);
            Assert.IsTrue(opt.HasValue);
            Assert.IsTrue(opt.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionalAccessUnsetValue()
        {
            var value = new Optional<int>().Value;
        }

        [TestMethod]
        public void OptionalAssignValue()
        {
            var opt = new Optional<int>();
            Assert.IsFalse(opt.HasValue);
            opt.Value = 5;
            Assert.IsTrue(opt.HasValue);
            Assert.AreEqual(opt.Value, 5);
        }

        [TestMethod]
        public void OptionalValueOr()
        {
            Assert.AreEqual(new Optional<int>().ValueOr(5), 5);
            Assert.AreEqual(new Optional<int>(5).ValueOr(10), 5);
        }

        [TestMethod]
        public void OptionalReset()
        {
            var opt = new Optional<string>("reset");
            Assert.AreEqual(opt.Value, "reset");
            opt.Reset();
            Assert.IsFalse(opt.HasValue);
        }

        [TestMethod]
        public void OptionalCastOperators()
        {
            var opt = new Optional<int>(5);
            int value = opt;
            Assert.AreEqual(value, 5);
            opt = 12;
            Assert.AreEqual(opt.Value, 12);
        }

        [TestMethod]
        public void OptionalEquals()
        {
            var opt = new Optional<int>(42);
            Assert.IsFalse(opt.Equals(null));
            Assert.IsFalse(opt.Equals(new object()));
            Assert.IsFalse(opt.Equals(new Optional<int>()));
            Assert.IsFalse(opt.Equals(new Optional<int>(13)));
            Assert.IsTrue(opt.Equals(new Optional<int>(42)));
        }

        [TestMethod]
        public void OptionalHashCode()
        {
            var opt1 = new Optional<int>(42);
            var opt2 = new Optional<int>(42);
            var opt3 = new Optional<char>('a');
            var opt4 = new Optional<float>();
            Assert.AreEqual(opt1.GetHashCode(), opt2.GetHashCode());
            Assert.AreNotEqual(opt1.GetHashCode(), opt3.GetHashCode());
            Assert.AreNotEqual(opt1.GetHashCode(), opt4.GetHashCode());
        }

        [TestMethod]
        public void OptionalToString()
        {
            var opt1 = new Optional<string>("Hello");
            var opt2 = new Optional<int>(11);
            Assert.AreEqual(opt1.ToString(), "Hello");
            Assert.AreEqual(opt2.ToString(), "11");
            opt2.Reset();
            Assert.AreNotEqual(opt2.ToString(), "11");
        }
    }
}
