using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimhashLib;

namespace SimhashTests
{
    [TestClass]
    public class TestHashing
    {
        [TestMethod]
        public void Hash_To_String_Value()
        {
            var simHash = new Simhash();
            var val = simHash.HashToString("aaa");
            Assert.AreEqual(val, "47bce5c74f589f4867dbd57e9ca9f808");
        }

        [TestMethod]
        public void Hash_String_To_Big_Int()
        {
            var simHash = new Simhash();
            var actualBiggie = simHash.HashStringToBigNasty("47bce5c74f589f4867dbd57e9ca9f808");
            var expectedBiggie = "95355999972893604581396806948474189832";
            Assert.AreEqual(expectedBiggie, actualBiggie.ToString());
        }
    }
}
