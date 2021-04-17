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
            var simHash = new SimhashLib.Simhash();

            var hash = simHash.GenerateSimhash("aaa bbb test test testing.happy time = -).");

            Assert.Equal(new SimhashResult(5683413558821905382), hash);
        }

        //Exact tests from https://github.com/liangsun/simhash
        [Fact]
        public void Value()
        {
            var features = new List<string>() { "aaa", "bbb" };
            var simHash = new SimhashLib.Simhash();
            
            var res = simHash.ComputeHash(features);
            
            var expected = new SimhashResult(8637903533912358349);
            Assert.Equal(expected, res);
        }
        
        [Fact]
        public void Value_Control()
        {
            var features = new List<string>() { "aaa" };
            var simHash = new SimhashLib.Simhash();
            
            var res = simHash.ComputeHash(features);
            
            Assert.Equal(new SimhashResult(7483809945577191432), res);
        }

        [Fact]
        public void Distance()
        {
            var sh = new SimhashLib.Simhash();
            
            var hash1 = sh.GenerateSimhash("How are you? I AM fine. Thanks. And you?");
            var hash2 = sh.GenerateSimhash("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            Assert.True(distA > 0);
            
            var distB = hash2.Distance(hash2);
            Assert.Equal(0,distB);

            var hash3 = sh.GenerateSimhash("1");
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        [Fact]
        public void Chinese()
        {
            var simhash = new SimhashLib.Simhash();
            var h1 = simhash.GenerateSimhash("你好　世界！　　呼噜。");
      
            var h2 = simhash.GenerateSimhash("你好，世界呼噜");
            Assert.Equal(0, h1.Distance(h2));

            var h4 = simhash.GenerateSimhash("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var h5 = simhash.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var h6 = simhash.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }

        [Fact]
        public void Short()
        {
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
            var simHash = new SimhashLib.Simhash();

            var shs = ss.Select(s => simHash.GenerateSimhash(s)).ToList();

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
