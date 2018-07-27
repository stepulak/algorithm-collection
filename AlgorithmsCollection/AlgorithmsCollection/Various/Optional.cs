using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsCollection
{
    public class Optional<T>
    {
        public bool HasValue { get; private set; }
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

        public Optional()
        {
            HasValue = false;
        }

        public Optional(T value)
        {
            Value = value;
        }

        public T ValueOr(T secondaryValue) => HasValue ? value : secondaryValue;

        public void Reset()
        {
            HasValue = false;
            value = default(T);
        }

        public static implicit operator T(Optional<T> opt) => opt.Value;
        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Optional<T>)
            {
                var opt = obj as Optional<T>;
                if (!HasValue)
                {
                    return opt.HasValue == false;
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
