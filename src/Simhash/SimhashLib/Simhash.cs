﻿using System.Collections.Generic;
using System.Text;
using SimhashLib.Abstraction;

namespace SimhashLib
{
    public readonly struct Simhash : IHash<SimhashResult>
    {
        private static readonly ulong[] Mask = BuildMask();
        
        public const int FpSize = 64;
        
        public SimhashResult ComputeHash(string content)
        {
            var builder = new StringBuilder(content.Length);
            var shingles = Shingling.Tokenize(content, builder);
            return ComputeHash(shingles);
        }
        
        public SimhashResult ComputeHash(List<string> features)
        {
            return ComputeHash<Md5Hash, Md5HashResult>(features, new Md5Hash());
        }

        public SimhashResult ComputeHash<THash, TRes>(List<string> features, THash hash)
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var fingerprint = new int[FpSize];

            foreach (var feature in features)
            {
                var h = hash.ComputeHash(feature);
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
            ulong ans = 0;
            for (var i = 0; i < FpSize; i++)
            {
                if (fingerprint[i] >= 0)
                    ans |= Mask[i];
            }

            return new SimhashResult(ans);
        }

        private static ulong[] BuildMask()
        {
            var masks = new ulong[FpSize];
            
            for (var i = 0; i < masks.Length; i++)
                masks[i] = (ulong) 1 << i;

            return masks;
        }
    }
}