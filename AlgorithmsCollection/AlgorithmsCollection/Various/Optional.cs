using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    /// <summary>
    /// Class that holds optional value. Equivalent to C++ optional class.
    /// </summary>
    /// <typeparam name="T">Type of optional value</typeparam>
    public class Optional<T>
    {
        /// <summary>
        /// Indicates whether this class has value present.
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Get/set optional value.
        /// </summary>
        public T Value
        {
            get
            {
                return HasValue ? value : throw new InvalidOperationException("Value not set");
            }
            set
            {
                HasValue = true;
                this.value = value;
            }
        }
        private T value;

        /// <summary>
        /// Default constructor with empty value. 
        /// </summary>
        public Optional()
        {
            HasValue = false;
        }

        /// <summary>
        /// Create instance of Optional class and fill it with given value.
        /// </summary>
        /// <param name="value">Value to set</param>
        public Optional(T value)
        {
            Value = value;
        }

        /// <summary>
        /// If HasValue is true, then return the primary value. Otherwise return given secondary value.
        /// </summary>
        /// <param name="secondaryValue">Secondary value to return if primary value is not set</param>
        /// <returns>Primary value if present, otherwise secondary value</returns>
        public T ValueOr(T secondaryValue) => HasValue ? value : secondaryValue;

        /// <summary>
        /// Remove value and set HasValue to false.
        /// </summary>
        public void Reset()
        {
            HasValue = false;
            value = default(T);
        }

        public static implicit operator T(Optional<T> opt) => opt.Value;
        public static implicit operator Optional<T>(T value) => new Optional<T>(value);
        
        public override bool Equals(object obj)
        {
            if (obj is Optional<T> opt)
            {
                if (!HasValue)
                {
                    return !opt.HasValue;
                }
                return opt.value.Equals(value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HasValue ? value.GetHashCode() : base.GetHashCode();
        }

        public override string ToString()
        {
            return HasValue ? value.ToString() : base.ToString();
        }
    }
}
