using System.Collections.Generic;
using System.Linq;
using SimhashLib;
using Xunit;

namespace Simhash.Test
{
    public class TestSimhash
    {
        [Fact]
        public void DistanceMd5()
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash1 = simhash.ComputeHashByMd5("How are you? I AM fine. Thanks. And you?");
            var hash2 = simhash.ComputeHashByMd5("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            Assert.True(distA > 0);
            
            var distB = hash2.Distance(hash2);
            Assert.Equal(0,distB);

            var hash3 = simhash.ComputeHashByMd5("1");
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        
        [Fact]
        public void ChineseMd5()
        {
            var simhash = new SimhashLib.Simhash();
            
            var h1 = simhash.ComputeHashByMd5("你好　世界！　　呼噜。");
      
            var h2 = simhash.ComputeHashByMd5("你好，世界呼噜");
            Assert.Equal(0, h1.Distance(h2));

            var h4 = simhash.ComputeHashByMd5("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var h5 = simhash.ComputeHashByMd5("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var h6 = simhash.ComputeHashByMd5("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }
        
        [Fact]
        public void ShortMd5()
        {
            var simhash = new SimhashLib.Simhash();
            
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
          
            var shs = ss.Select(s => simhash.ComputeHashByMd5(s)).ToList();

            foreach (var sh1 in shs)
            {
                foreach (var sh2 in shs.Where(sh2 => !sh1.Equals(sh2)))
                {
                    Assert.NotEqual(sh1, sh2);
                }
            }
        }
        
        [Fact]
        public void DistanceMurmurHash3()
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash1 = simhash.ComputeHashByMurmurHash3("How are you? I AM fine. Thanks. And you?");
            var hash2 = simhash.ComputeHashByMurmurHash3("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            var dist2 = hash1.Distance(hash2);
            Assert.True(distA > 0);
            Assert.True(dist2 > 0);
            
            var distB = hash2.Distance(hash2);
            var dist3 = hash2.Distance(hash2);
            Assert.Equal(0,distB);
            Assert.Equal(0,dist3);

            var hash3 = simhash.ComputeHashByMurmurHash3("1");
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        
        [Fact]
        public void ChineseMurmurHash3()
        {
            var simhash = new SimhashLib.Simhash();
            
            var h1 = simhash.ComputeHashByMurmurHash3("你好　世界！　　呼噜。");
      
            var h2 = simhash.ComputeHashByMurmurHash3("你好，世界呼噜");
            Assert.Equal(0, h1.Distance(h2));

            var h4 = simhash.ComputeHashByMurmurHash3("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var h5 = simhash.ComputeHashByMurmurHash3("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var h6 = simhash.ComputeHashByMurmurHash3("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }
        
        [Fact]
        public void ShortMurmurHash3()
        {
            var simhash = new SimhashLib.Simhash();
            
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
          
            var shs = ss.Select(s => simhash.ComputeHashByMurmurHash3(s)).ToList();

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
