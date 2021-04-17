using System.Collections.Generic;
using System.Linq;
using SimhashLib;
using Xunit;

namespace Simhash.Test
{
    public class TestSimhash
    {
        //only works with md5 hashing
        [Fact]
        public void Value_By_String()
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash = simhash.ComputeHash("aaa bbb test test testing.happy time = -).");

            Assert.Equal(new SimhashResult(5683413558821905382), hash);
        }

        //Exact tests from https://github.com/liangsun/simhash
        [Fact]
        public void Value()
        {
            var simhash = new SimhashLib.Simhash();
            
            var features = new List<string>() { "aaa", "bbb" };
      
            var res = simhash.ComputeHash(features);
            
            var expected = new SimhashResult(8637903533912358349);
            Assert.Equal(expected, res);
        }
        
        [Fact]
        public void Value_Control()
        {
            var simhash = new SimhashLib.Simhash();
            
            var features = new List<string>() { "aaa" };
            
            var res = simhash.ComputeHash(features);
            
            Assert.Equal(new SimhashResult(7483809945577191432), res);
        }

        [Fact]
        public void Distance()
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash1 = simhash.ComputeHash("How are you? I AM fine. Thanks. And you?");
            var hash2 = simhash.ComputeHash("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            Assert.True(distA > 0);
            
            var distB = hash2.Distance(hash2);
            Assert.Equal(0,distB);

            var hash3 = simhash.ComputeHash("1");
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        [Fact]
        public void Chinese()
        {
            var simhash = new SimhashLib.Simhash();
            
            var h1 = simhash.ComputeHash("你好　世界！　　呼噜。");
      
            var h2 = simhash.ComputeHash("你好，世界呼噜");
            Assert.Equal(0, h1.Distance(h2));

            var h4 = simhash.ComputeHash("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var h5 = simhash.ComputeHash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var h6 = simhash.ComputeHash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }

        [Fact]
        public void Short()
        {
            var simhash = new SimhashLib.Simhash();
            
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
          
            var shs = ss.Select(s => simhash.ComputeHash(s)).ToList();

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
