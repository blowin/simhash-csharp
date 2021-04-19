using System.Collections.Generic;
using System.Text;
using SimhashLib.Abstraction;

namespace SimhashLib
{
    public readonly struct Simhash
    {
        private static readonly long[] Mask = BuildMask();
        
        public const int FpSize = 64;

        public SimhashResult ComputeHash<THash, TRes>(string content, THash hash, Encoding encoding = null) 
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var builder = new StringBuilder(content.Length);
            var shingles = Shingling.Tokenize(content, builder);
            return ComputeHash<THash, TRes>(shingles, hash, encoding);
        } 
    
        public SimhashResult ComputeHash<THash, TRes>(List<string> tokens, THash hash, Encoding encoding = null)
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            encoding ??= Encoding.UTF8;
            
            var fingerprint = new int[FpSize];
            
            foreach (var feature in tokens)
            {
                var bytes = encoding.GetBytes(feature);
                var h = hash.ComputeHash(bytes);
                const int w = 1;
                for (var i = 0; i < FpSize; i++)
                {
                    var bMask = Mask[i];
                    var result = h.BitwiseAnd(bMask);
                    fingerprint[i] += result.GreatThanZero ? w : -w;
                }
            }

            return FingerprintToSimhashResult(fingerprint);
        }
        
        private static SimhashResult FingerprintToSimhashResult(int[] fingerprint)
        {
            long ans = 0;
            for (var i = 0; i < FpSize; i++)
            {
                if (fingerprint[i] >= 0)
                    ans |= Mask[i];
            }

            return new SimhashResult(unchecked((ulong)ans));
        }

        private static long[] BuildMask()
        {
            var masks = new long[FpSize];
            
            for (var i = 0; i < masks.Length; i++)
                masks[i] = (long) 1 << i;

            return masks;
        }
    }
}