using System;
using SimhashLib.Abstraction;

namespace SimhashLib
{
    public readonly struct SimhashResult : IEquatable<SimhashResult>, IHashResult<SimhashResult>
    {
        public ulong Value { get; }

        public bool GreatThanZero => Value > 0;
        
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
        
        public SimhashResult BitwiseAnd(ulong mask) => new SimhashResult(Value & mask);
        
        public override string ToString() => Value.ToString();
    }
}