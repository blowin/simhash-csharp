﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var hash = simHash.GenerateSimhash("aaa bbb test test testing.happy time = -).");

            Assert.AreEqual(new Simhash.Hash(5683413558821905382), hash);
        }

        //Exact tests from https://github.com/liangsun/simhash
        [TestMethod]
        public void Value()
        {
            var features = new List<string>() { "aaa", "bbb" };
            var simHash = new Simhash();
            
            var res = simHash.ComputeHash(features);
            
            var expected = new Simhash.Hash(8637903533912358349);
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void Value_Control()
        {
            var features = new List<string>() { "aaa" };
            var simHash = new Simhash();
            
            var res = simHash.ComputeHash(features);
            
            Assert.AreEqual(new Simhash.Hash(7483809945577191432), res);
        }

        [TestMethod]
        public void Distance()
        {
            var sh = new Simhash();
            var sh2 = new Simhash();
            
            var hash1 = sh.GenerateSimhash("How are you? I AM fine. Thanks. And you?");
            var hash2 = sh2.GenerateSimhash("How old are you? :-) i am fine. Thanks. And you?");
            var distA = hash1.Distance(hash2);
            Assert.IsTrue(distA > 0);
            
            var distB = hash2.Distance(hash2);
            Assert.AreEqual(0,distB);

            var sh4 = new Simhash();
            var hash3 = sh4.GenerateSimhash("1");
            
            Assert.AreNotEqual(0, hash3.Distance(hash2));
        }
        [TestMethod]
        public void Chinese()
        {
            var sh = new Simhash();
            var h1 = sh.GenerateSimhash("你好　世界！　　呼噜。");
            var sh2 = new Simhash();
            var h2 = sh2.GenerateSimhash("你好，世界呼噜");
            Assert.AreEqual(h1.Distance(h2), 0);

            var sh4 = new Simhash();
            var h4 = sh4.GenerateSimhash("How are you? I Am fine. ablar ablar xyz blar blar blar blar blar blar blar Thanks.");
            var sh5 = new Simhash();
            var h5 = sh5.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar than");
            var sh6 = new Simhash();
            var h6 = sh6.GenerateSimhash("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");

            Assert.IsTrue(h4.Distance(h6) < 3);
            Assert.IsTrue(h5.Distance(h6) < 3);
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
