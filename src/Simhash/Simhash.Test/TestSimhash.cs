using System.Collections.Generic;
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

            Assert.Equal(new SimhashLib.Simhash.Hash(5683413558821905382), hash);
        }

        //Exact tests from https://github.com/liangsun/simhash
        [Fact]
        public void Value()
        {
            var features = new List<string>() { "aaa", "bbb" };
            var simHash = new SimhashLib.Simhash();
            
            var res = simHash.ComputeHash(features);
            
            var expected = new SimhashLib.Simhash.Hash(8637903533912358349);
            Assert.Equal(expected, res);
        }
        
        [Fact]
        public void Value_Control()
        {
            var features = new List<string>() { "aaa" };
            var simHash = new SimhashLib.Simhash();
            
            var res = simHash.ComputeHash(features);
            
            Assert.Equal(new SimhashLib.Simhash.Hash(7483809945577191432), res);
        }

        [Fact]
        public void Distance()
        {
            var sh = new SimhashLib.Simhash();
            var sh2 = new SimhashLib.Simhash();
            
            var hash1 = sh.GenerateSimhash("How are you? I AM fine. Thanks. And you?");
            var hash2 = sh2.GenerateSimhash("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            Assert.True(distA > 0);
            
            var distB = hash2.Distance(hash2);
            Assert.Equal(0,distB);

            var sh4 = new SimhashLib.Simhash();
            var hash3 = sh4.GenerateSimhash("1");
            
            Assert.NotEqual(0, hash3.Distance(hash2));
        }
        [Fact]
        public void Chinese()
        {
            var sh = new SimhashLib.Simhash();
            var h1 = sh.GenerateSimhash("你好　世界！　　呼噜。");
            var sh2 = new SimhashLib.Simhash();
            var h2 = sh2.GenerateSimhash("你好，世界呼噜");
            Assert.Equal(0, h1.Distance(h2));

            var sh4 = new SimhashLib.Simhash();
            var h4 = sh4.GenerateSimhash("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var sh5 = new SimhashLib.Simhash();
            var h5 = sh5.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var sh6 = new SimhashLib.Simhash();
            var h6 = sh6.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.True(h4.Distance(h6) < 3);
            Assert.True(h5.Distance(h6) < 3);
        }

        [Fact]
        public void Short()
        {
            var shs = new List<SimhashLib.Simhash>();
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
            foreach (var s in ss)
            {
                var simHash = new SimhashLib.Simhash();
                simHash.GenerateSimhash(s);
                shs.Add(simHash);
            }

            foreach (var sh1 in shs)
            {
                foreach (var sh2 in shs)
                {
                    if (sh1 != sh2)
                    {
                        Assert.NotEqual(sh1, sh2);
                    }
                }
            }
        }
    }
}
