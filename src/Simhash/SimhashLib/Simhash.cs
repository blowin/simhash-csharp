using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace SimhashLib
{
    public class Simhash
    {
        public const int FpSize = 64;
       
        public ulong Value { get; set; }

        public Simhash()
        {
        }

        public Simhash(Simhash simHash)
        {
            Value = simHash.Value;
        }

        public Simhash(ulong fingerPrint)
        {
            Value = fingerPrint;
        }

        public void GenerateSimhash(string content)
        {
            var shingles = Shingling.Tokenize(content);
            ComputeHash(shingles);
        }

        public void ComputeHash(List<string> features)
        {
            BuildByFeaturesMd5(features);
        }

        public long GetFingerprintAsLong()
        {
            return Converters.ConvertUlongToLong(Value);
        }

        public int Distance(Simhash another)
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

        private void BuildByFeaturesMd5(List<string> features)
        {
            var v = SetupFingerprint();
            var masks = SetupMasks();

            foreach (var feature in features)
            {
                //this is using MD5 which is REALLY slow
                var h = HashFuncMd5(feature);
                var w = 1;
                for (var i = 0; i < FpSize; i++)
                {
                    //convert to BigInt so we can use BitWise
                    BigInteger bMask = masks[i];
                    var result = h & bMask;
                    v[i] += (result > 0) ? w : -w;
                }
            }

            Value = MakeFingerprint(v, masks);
        }


        private ulong MakeFingerprint(int[] v, ulong[] masks)
        {
            ulong ans = 0;
            for (var i = 0; i < FpSize; i++)
            {
                if (v[i] >= 0)
                {
                    ans |= masks[i];
                }
            }

            return ans;
        }

        private int[] SetupFingerprint()
        {
            var v = new int[FpSize];
            for (var i = 0; i < v.Length; i++) v[i] = 0;
            return v;
        }

        private ulong[] SetupMasks()
        {
            var masks = new ulong[FpSize];
            for (var i = 0; i < masks.Length; i++)
            {
                masks[i] = (ulong) 1 << i;
            }

            return masks;
        }

        private BigInteger HashFuncMd5(string x)
        {
            var hexValue = HashToString(x);
            var b = HashStringToBigNasty(hexValue);
            return b;
        }

        public string HashToString(string x)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(x));

                var returnString = "";
                for (var i = 0; i < data.Length; i++)
                {
                    returnString += data[i].ToString("x2");
                }

                return returnString;
            }
        }

        public BigInteger HashStringToBigNasty(string x)
        {
            var bigNumber = BigInteger.Parse(x, NumberStyles.AllowHexSpecifier);
            return bigNumber;
        }
    }
}