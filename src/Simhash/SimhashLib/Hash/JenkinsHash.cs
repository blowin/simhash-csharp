using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimhashLib.Abstraction;

namespace SimhashLib.Hash
{
    public readonly struct JenkinsHashResult : IHashResult<JenkinsHashResult>
    {
        private readonly uint _val;

        public bool GreatThanZero => _val > 0;
        
        public JenkinsHashResult(uint val)
        {
            _val = val;
        }

        public JenkinsHashResult BitwiseAnd(long mask) => new JenkinsHashResult((uint)(_val & mask));
    }
    
    // https://github.com/brandondahler/Data.HashFunction/blob/master/src/OpenSource.Data.HashFunction.Jenkins/JenkinsLookup3_Implementation.cs
    public readonly struct JenkinsHash : IHash<JenkinsHashResult>
    {
        private readonly uint _seed;

        public JenkinsHash(uint seed)
        {
            _seed = seed;
        }

        public JenkinsHashResult ComputeHash(byte[] content)
        {
            var a = 0xdeadbeef + (uint) content.Length + _seed;
            var b = a;
            var c = a;
            
            var dataArray = content;
            var dataOffset = 0;
            var dataCount = content.Length;

            var remainderCount = dataCount % 12;
            {
                if (remainderCount == 0 && dataCount > 0)
                    remainderCount = 12;
            }

            var remainderOffset = dataOffset + dataCount - remainderCount;

            // Main group processing
            var currentOffset = dataOffset;
            {
                while (currentOffset < remainderOffset)
                {
                    a += BitConverter.ToUInt32(dataArray, currentOffset);
                    b += BitConverter.ToUInt32(dataArray, currentOffset + 4);
                    c += BitConverter.ToUInt32(dataArray, currentOffset + 8);

                    Mix(ref a, ref b, ref c);

                    currentOffset += 12;
                }
            }

            // Remainder processing
            {
                Debug.Assert(remainderCount >= 0);
                Debug.Assert(remainderCount <= 12);

                switch (remainderCount)
                {
                    case 12:
                        c += BitConverter.ToUInt32(dataArray, currentOffset + 8);
                        goto case 8;

                    case 11: c += (uint) dataArray[currentOffset + 10] << 16; goto case 10;
                    case 10: c += (uint) dataArray[currentOffset + 9] << 8; goto case 9;
                    case 9:  c += (uint) dataArray[currentOffset + 8]; goto case 8;

                    case 8:
                        b += BitConverter.ToUInt32(dataArray, currentOffset + 4);
                        goto case 4;

                    case 7: b += (uint) dataArray[currentOffset + 6] << 16; goto case 6;
                    case 6: b += (uint) dataArray[currentOffset + 5] << 8; goto case 5;
                    case 5: b += (uint) dataArray[currentOffset + 4]; goto case 4;

                    case 4:
                        a += BitConverter.ToUInt32(dataArray, currentOffset);

                        Final(ref a, ref b, ref c);
                        break;

                    case 3: a += (uint) dataArray[currentOffset + 2] << 16; goto case 2;
                    case 2: a += (uint) dataArray[currentOffset + 1] << 8; goto case 1;
                    case 1:
                        a += (uint) dataArray[currentOffset];

                        Final(ref a, ref b, ref c);
                        break;
                }
            }
            
            return new JenkinsHashResult(c);
        }

        private static void Mix(ref uint a, ref uint b, ref uint c)
        {
            a -= c; a ^= RotateLeft(c, 4); c += b;
            b -= a; b ^= RotateLeft(a,  6); a += c;
            c -= b; c ^= RotateLeft(b,  8); b += a;

            a -= c; a ^= RotateLeft(c, 16); c += b;
            b -= a; b ^= RotateLeft(a, 19); a += c;
            c -= b; c ^= RotateLeft(b,  4); b += a;
        }

        private static void Final(ref uint a, ref uint b, ref uint c)
        {
            c ^= b; c -= RotateLeft(b, 14);
            a ^= c; a -= RotateLeft(c, 11);
            b ^= a; b -= RotateLeft(a, 25);

            c ^= b; c -= RotateLeft(b, 16);
            a ^= c; a -= RotateLeft(c,  4);
            b ^= a; b -= RotateLeft(a, 14);

            c ^= b; c -= RotateLeft(b, 24);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RotateLeft(uint operand, int shiftCount)
        {
            shiftCount &= 0x1f;

            return (operand << shiftCount) | (operand >> (32 - shiftCount));
        }
    }
}