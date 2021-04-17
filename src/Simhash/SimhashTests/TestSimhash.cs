using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SimhashLib;

namespace SimhashTests
{
    [TestClass]
    public class TestSimhash
    {
        //only works with md5 hashing
        [TestMethod]
        public void Value_By_String()
        {
            var simHash = new Simhash();
            simHash.GenerateSimhash("aaa bbb test test testing.happy time = -).");
            ulong expected = 5683413558821905382;
            Assert.AreEqual(expected, simHash.Value);
        }

        //Exact tests from https://github.com/liangsun/simhash
        [TestMethod]
        public void Value()
        {
            var features = new List<string>() { "aaa", "bbb" };
            var simHash = new Simhash();
            simHash.ComputeHash(features);
            ulong expected = 8637903533912358349;
            Assert.AreEqual(expected, simHash.Value);
        }
        
        [TestMethod]
        public void Value_Control()
        {
            var features = new List<string>() { "aaa" };
            var simHash = new Simhash();
            simHash.ComputeHash(features);
            ulong expected = 7483809945577191432;
            Assert.AreEqual(expected, simHash.Value);
        }

        [TestMethod]
        public void Distance()
        {
            var sh = new Simhash();
            sh.GenerateSimhash("How are you? I AM fine. Thanks. And you?");
            var sh2 = new Simhash();
            sh2.GenerateSimhash("How old are you? :-) i am fine. Thanks. And you?");
            var distA = sh.Distance(sh2);
            Assert.IsTrue(distA > 0);

            var sh3 = new Simhash(sh2);
            var distB = sh2.Distance(sh3);
            Assert.AreEqual(0,distB);

            var sh4 = new Simhash();
            sh4.GenerateSimhash("1");
            Assert.AreNotEqual(0, sh4.Distance(sh3));
        }
        [TestMethod]
        public void Chinese()
        {
            var sh = new Simhash();
            sh.GenerateSimhash("你好　世界！　　呼噜。");
            var sh2 = new Simhash();
            sh2.GenerateSimhash("你好，世界呼噜");
            Assert.AreEqual(sh.Distance(sh2), 0);

            var sh4 = new Simhash();
            sh4.GenerateSimhash("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var sh5 = new Simhash();
            sh5.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var sh6 = new Simhash();
            sh6.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.IsTrue(sh4.Distance(sh6) < 3);
            Assert.IsTrue(sh5.Distance(sh6) < 3);
        }

        [TestMethod]
        public void Short()
        {
            var shs = new List<Simhash>();
            var ss = new List<string>() { "aa", "aaa", "aaaa", "aaaab", "aaaaabb", "aaaaabbb" };
            foreach (var s in ss)
            {
                var simHash = new Simhash();
                simHash.GenerateSimhash(s);
                shs.Add(simHash);
            }

            foreach (var sh1 in shs)
            {
                foreach (var sh2 in shs)
                {
                    if (sh1 != sh2)
                    {
                        Assert.AreNotEqual(sh1, sh2);
                    }
                }
            }
        }
    }
}
