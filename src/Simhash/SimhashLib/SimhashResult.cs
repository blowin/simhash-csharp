using System;
using SimhashLib.Abstraction;

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
            const long m1  = 0x5555555555555555; //binary: 0101...
            const long m2  = 0x3333333333333333; //binary: 00110011..
            const long m4  = 0x0f0f0f0f0f0f0f0f; //binary:  4 zeros,  4 ones ...
            const long h01 = 0x0101010101010101; //the sum of 256 to the power of 0,1,2,3...
            
            var x = (Value ^ another.Value) & (ulong.MaxValue);
            x -= (x >> 1) & m1;             //put count of each 2 bits into those 2 bits
            x = (x & m2) + ((x >> 2) & m2); //put count of each 4 bits into those 4 bits 
            x = (x + (x >> 4)) & m4;        //put count of each 8 bits into those 8 bits 
            return (int)((x * h01) >> 56);  //returns left 8 bits of x + (x<<8) + (x<<16) + (x<<24) + ... 
        }
        
        public override string ToString() => Value.ToString();
    }
}