using System.Collections.Generic;
using System.Text;
using SimhashLib.Abstraction;

namespace SimhashLib
{
    public readonly struct Simhash : IHash<SimhashResult>
    {
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
            var fingerprint = BuildFingerprint();
            var masks = BuildMask();

            foreach (var feature in features)
            {
                //this is using MD5 which is REALLY slow
                var h = hash.ComputeHash(feature);
                const int w = 1;
                for (var i = 0; i < FpSize; i++)
                {
                    //convert to BigInt so we can use BitWise
                    var bMask = masks[i];
                    var result = h.BitwiseAnd(bMask);
                    fingerprint[i] += result.GreatThanZero ? w : -w;
                }
            }

            return MakeFingerprint(fingerprint, masks);
        }
        
        private static SimhashResult MakeFingerprint(int[] v, ulong[] masks)
        {
            ulong ans = 0;
            for (var i = 0; i < FpSize; i++)
            {
                if (v[i] >= 0)
                    ans |= masks[i];
            }

            return new SimhashResult(ans);
        }

        private static int[] BuildFingerprint()
        {
            var v = new int[FpSize];
            for (var i = 0; i < v.Length; i++) 
                v[i] = 0;
            return v;
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