using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using SimhashLib.Abstraction;

namespace SimhashLib.Hash
{
    public readonly struct Md5HashResult : IHashResult<Md5HashResult>
    {
        private readonly BigInteger _val;

        public bool GreatThanZero => _val > 0;
        
        public Md5HashResult(BigInteger val)
        {
            _val = val;
        }

        public Md5HashResult BitwiseAnd(ulong mask) => new Md5HashResult(_val & mask);
    }
    
    public readonly struct Md5Hash : IHash<Md5HashResult>
    {
        public Md5HashResult ComputeHash(string feature)
        {
            //this is using MD5 which is REALLY slow
            var hexValue = HashToString(feature);
            var nasty = HashStringToBigNasty(hexValue);
            return new Md5HashResult(nasty);
        }
        
        public static string HashToString(string x)
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