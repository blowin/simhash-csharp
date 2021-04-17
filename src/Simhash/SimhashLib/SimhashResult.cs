using System;

namespace SimhashLib
{
    public readonly struct SimhashResult : IEquatable<SimhashResult>
    {
        public ulong Value { get; }

        public SimhashResult(ulong value)
        {
            Value = value;
        }

        public bool Equals(SimhashResult other) => Value == other.Value;

        public override bool Equals(object obj) => obj is SimhashResult other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public int Distance(SimhashResult another)
        {
            var x = (Value ^ another.Value) & (ulong.MaxValue);
            var ans = 0;
            
            while (x > 0)
            {
                ans++;
                x &= x - 1;
            }

            return ans;
        }
        
        public override string ToString() => Value.ToString();
    }
}