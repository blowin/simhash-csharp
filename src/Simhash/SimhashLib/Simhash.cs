using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace SimhashLib
{
    public sealed class Simhash
    {
        public const int FpSize = 64;
        
        public SimhashResult GenerateSimhash(string content)
        {
            var builder = new StringBuilder(content.Length);
            var shingles = Shingling.Tokenize(content, builder);
            return ComputeHash(shingles);
        }

        public SimhashResult ComputeHash(List<string> features)
        {
            var fingerprint = BuildFingerprint();
            var masks = BuildMask();

            foreach (var feature in features)
            {
                //this is using MD5 which is REALLY slow
                var h = HashFuncMd5(feature);
                const int w = 1;
                for (var i = 0; i < FpSize; i++)
                {
                    //convert to BigInt so we can use BitWise
                    BigInteger bMask = masks[i];
                    var result = h & bMask;
                    fingerprint[i] += (result > 0) ? w : -w;
                }
            }

            return MakeFingerprint(fingerprint, masks);
        }

        private SimhashResult MakeFingerprint(int[] v, ulong[] masks)
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

        private BigInteger HashFuncMd5(string x)
        {
            var hexValue = HashToString(x);
            return HashStringToBigNasty(hexValue);
        }

        public string HashToString(string x)
        {
            using var md5Hash = MD5.Create();
            
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(x));

            var result = new StringBuilder(data.Length * 2);
            foreach (var value in data)
                result.AppendFormat("{0:x2}", value);

            return result.ToString();
        }

        public static BigInteger HashStringToBigNasty(string x) => BigInteger.Parse(x, NumberStyles.AllowHexSpecifier);
    }
}