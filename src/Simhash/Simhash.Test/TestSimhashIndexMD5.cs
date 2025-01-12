﻿using System.Collections.Generic;
using SimhashLib;
using Xunit;

namespace Simhash.Test
{
    public class TestSimhashIndexMD5
    {
        private Dictionary<long, SimhashResult> objs = new Dictionary<long, SimhashResult>();
        private SimhashIndex index;
        private Dictionary<long, string> testData = new Dictionary<long, string>();
        
        public TestSimhashIndexMD5()
        {
            var simhash = new SimhashLib.Simhash();
            
            testData.Add(1, "How are you? I Am fine. blar blar blar blar blar Thanks.");
            testData.Add(2, "How are you i am fine. blar blar blar blar blar than");
            testData.Add(3, "This is simhash test.");
            testData.Add(4, "How are you i am fine. blar blar blar blar blar thank1");
            
            foreach(var it in testData)
            {
                objs.Add(it.Key, simhash.ComputeHashByMd5(it.Value));
            }
            index = new SimhashIndex(objs: objs, k: 10);
           
        }
        
        [Fact]
        public void Offset_Creation_With_Ten()
        {
            var dict = new Dictionary<long, SimhashResult>();
            var simHashIndex = new SimhashIndex(dict, k: 10);
            
            var offsets = simHashIndex.MakeOffsets();
            
            Assert.Equal(0, offsets[0]);
            Assert.Equal(10, offsets[2]);
            Assert.True(offsets.Count == 11);
        }
        
        [Fact]
        public void Offset_Creation_With_Two()
        {
            var dict = new Dictionary<long, SimhashResult>();
            var simHashIndex = new SimhashIndex(dict, k: 2);
            
            var offsets = simHashIndex.MakeOffsets();
            
            Assert.Equal(0, offsets[0]);
            Assert.Equal(42, offsets[2]);
            Assert.True(offsets.Count == 3);
        }

        [Fact]
        public void Get_Keys()
        {
            var simhash = new SimhashLib.Simhash();
            
            var testdata = new Dictionary<long, string>();
            testdata.Add(1, "How are you? I Am fine. blar blar blar blar blar Thanks.");

            var simHashObjs = new Dictionary<long, SimhashResult>();
            foreach (var it in testdata)
            {
                simHashObjs.Add(it.Key, simhash.ComputeHashByMd5(it.Value));
            }
            var simHashIndex = new SimhashIndex(objs: simHashObjs, k: 10);
            var listOfKeys = simHashIndex.GetListKeys(simHashObjs[1]);
            Assert.True(listOfKeys.Count == 11);
            Assert.Equal("26,0", listOfKeys[0]);
            Assert.Equal("3,1", listOfKeys[1]);
            Assert.Equal("7,2", listOfKeys[2]);
            Assert.Equal("12,3", listOfKeys[3]);
            Assert.Equal("17,4", listOfKeys[4]);
            Assert.Equal("0,5", listOfKeys[5]);
            Assert.Equal("13,6", listOfKeys[6]);
            Assert.Equal("30,7", listOfKeys[7]);
            Assert.Equal("1,8", listOfKeys[8]);
            Assert.Equal("14,9", listOfKeys[9]);
            Assert.Equal("7496,10", listOfKeys[10]);

        }

        [Fact]
        public void Get_Near_Dup_Hash()
        {
            var simhash = new SimhashLib.Simhash();
            
            var hash = simhash.ComputeHashByMd5("How are you i am fine.ablar ablar xyz blar blar blar blar blar blar blar thank");
            var dups = index.GetNearDups(hash);
            Assert.Equal(3, dups.Count);

            var hash2 = simhash.ComputeHashByMd5(testData[1]);
            index.Delete(1, hash2);
            dups = index.GetNearDups(hash);
            Assert.Equal(2, dups.Count);

            var hash3 = simhash.ComputeHashByMd5(testData[1]);
            index.Delete(1, hash3);
            dups = index.GetNearDups(hash);
            Assert.Equal(2, dups.Count);

            var hash4 = simhash.ComputeHashByMd5(testData[1]);
            index.Add(1, hash4);
            dups = index.GetNearDups(hash);
            Assert.Equal(3, dups.Count);

            var hash5 = simhash.ComputeHashByMd5(testData[1]);
            index.Add(1, hash5);
            dups = index.GetNearDups(hash);
            Assert.Equal(3, dups.Count);
        }
    }
}
