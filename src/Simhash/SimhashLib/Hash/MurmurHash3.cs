using System;
using SimhashLib.Abstraction;

namespace SimhashLib.Hash
{
    public readonly struct MurmurHash3Result : IHashResult<MurmurHash3Result>
    {
        private readonly long _val;

        public bool GreatThanZero => _val > 0;
        
        public MurmurHash3Result(long val)
        {
            _val = val;
        }

        public MurmurHash3Result BitwiseAnd(long mask) => new MurmurHash3Result(_val & mask);
    }
    
    public readonly struct MurmurHash3 : IHash<MurmurHash3Result>
    {
        private const uint M = 0x5bd1e995;
        private const int R = 24;
        
        public MurmurHash3Result ComputeHash(byte[] content)
        {
            var result = Hash(content, 0xc58f1a7a);
            return new MurmurHash3Result(result);
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