using System;
using SimhashLib.Abstraction;

namespace SimhashLib.Hash
{
    public readonly struct Murmur3HashResult : IHashResult<Murmur3HashResult>
    {
        private readonly long _val;

        public bool GreatThanZero => _val > 0;
        
        public Murmur3HashResult(long val)
        {
            _val = val;
        }

        public Murmur3HashResult BitwiseAnd(long mask) => new Murmur3HashResult(_val & mask);
    }
    
    public readonly struct MurmurHash3 : IHash<Murmur3HashResult>
    {
        const uint M = 0x5bd1e995;
        const int R = 24;
        
        public Murmur3HashResult ComputeHash(byte[] content)
        {
            var result = Hash(content, 0xc58f1a7a);
            return new Murmur3HashResult(result);
        }
        
        public static uint Hash(byte[] data, uint seed)
        {
            int length = data.Length;
            if (length == 0)
                return 0;
            uint h = seed ^ (uint)length;
            int currentIndex = 0;
            while (length >= 4)
            {
                uint k = (uint)(data[currentIndex++] | data[currentIndex++] << 8 | data[currentIndex++] << 16 | data[currentIndex++] << 24);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }
            switch (length)
            {
                case 3:
                    h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
                    h ^= (uint)(data[currentIndex] << 16);
                    h *= M;
                    break;
                case 2:
                    h ^= (UInt16)(data[currentIndex++] | data[currentIndex] << 8);
                    h *= M;
                    break;
                case 1:
                    h ^= data[currentIndex];
                    h *= M;
                    break;
                default:
                    break;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return h;
        }
    }
}