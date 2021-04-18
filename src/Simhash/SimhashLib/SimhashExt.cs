using System.Collections.Generic;
using System.Text;
using SimhashLib.Abstraction;

namespace SimhashLib
{
    public static class SimhashExt
    {
        public static SimhashResult ComputeHashByMd5(this Simhash self, string content)
            => self.ComputeHash<Md5Hash, Md5HashResult>(content);
        
        public static SimhashResult ComputeHashByMd5(this Simhash self, List<string> tokens) 
            => self.ComputeHash<Md5Hash, Md5HashResult>(tokens);
        
        private static SimhashResult ComputeHash<THash, TRes>(this Simhash self, string content) 
            where THash : struct, IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var builder = new StringBuilder(content.Length);
            var shingles = Shingling.Tokenize(content, builder);
            return self.ComputeHash<THash, TRes>(shingles);
        }
    }
}