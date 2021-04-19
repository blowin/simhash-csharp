using System.Collections.Generic;
using System.Linq;
using SimhashLib;
using SimhashLib.Abstraction;
using SimhashLib.Hash;
using Xunit;

namespace Simhash.Test
{
    public class TestSimhash
    {
        [Fact]
        public void DistanceMd5()
        {
            Distance<Md5Hash, Md5HashResult>(new Md5Hash());
        }
        
        [Fact]
        public void ChineseMd5()
        {
            Chinese<Md5Hash, Md5HashResult>(new Md5Hash());
        }
        
        [Fact]
        public void ShortMd5()
        {
            Short<Md5Hash, Md5HashResult>(new Md5Hash());
        }
        
        [Fact]
        public void DistanceMurmurHash3()
        {
            Distance<MurmurHash3, MurmurHash3Result>(new MurmurHash3());
        }
        
        [Fact]
        public void ChineseMurmurHash3()
        {
            Chinese<MurmurHash3, MurmurHash3Result>(new MurmurHash3());
        }
        
        [Fact]
        public void ShortMurmurHash3()
        {
            Short<MurmurHash3, MurmurHash3Result>(new MurmurHash3());
        }
        
        [Fact]
        public void DistanceJenkinsHash()
        {
            Distance<JenkinsHash, JenkinsHashResult>(new JenkinsHash());
        }
        
        [Fact]
        public void ChineseJenkinsHash()
        {
            Chinese<JenkinsHash, JenkinsHashResult>(new JenkinsHash());
        }
        
        [Fact]
        public void ShortJenkinsHash()
        {
            Short<JenkinsHash, JenkinsHashResult>(new JenkinsHash());
        }
        
        public void Distance<THash, TRes>(THash hash) 
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash1 = simhash.ComputeHash<THash, TRes>("How are you? I AM fine. Thanks. And you?", hash);
            var hash2 = simhash.ComputeHash<THash, TRes>("How old are you? :-) i am fine. Thanks. And you?", hash);
            var distA = hash1.Distance(hash2);
            var dist2 = hash1.Distance(hash2);
            Assert.True(distA > 0);
            Assert.True(dist2 > 0);
            
            var distB = hash2.Distance(hash2);
            var dist3 = hash2.Distance(hash2);
            Assert.Equal(0,distB);
            Assert.Equal(0,dist3);

            var hash3 = simhash.ComputeHash<THash, TRes>("1", hash);
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        
        private void Chinese<THash, TRes>(THash hash) 
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var simhash = new SimhashLib.Simhash();
            
            var h1 = simhash.ComputeHash<THash, TRes>("你好　世界！　　呼噜。", hash);
      
            var h2 = simhash.ComputeHash<THash, TRes>("你好，世界呼噜", hash);
            Assert.Equal(0, h1.Distance(h2));

            var h4 = simhash.ComputeHash<THash, TRes>("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.", hash);
            var h5 = simhash.ComputeHash<THash, TRes>("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than", hash);
            var h6 = simhash.ComputeHash<THash, TRes>("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank", hash);

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }
        
        private void Short<THash, TRes>(THash hash) 
            where THash : IHash<TRes> 
            where TRes : IHashResult<TRes>
        {
            var simhash = new SimhashLib.Simhash();
            
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
          
            var shs = ss.Select(s => simhash.ComputeHash<THash, TRes>(s, hash)).ToList();

            foreach (var sh1 in shs)
            {
                foreach (var sh2 in shs.Where(sh2 => !sh1.Equals(sh2)))
                {
                    Assert.NotEqual(sh1, sh2);
                }
            }
        }
    }
}
