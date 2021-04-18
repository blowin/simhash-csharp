using SimhashLib;
using SimhashLib.Hash;
using Xunit;

namespace Simhash.Test
{
    public class TestHashing
    {
        [Fact]
        public void Hash_To_String_Value()
        {
            var val = Md5Hash.HashToString("aaa");
            Assert.Equal("47bce5c74f589f4867dbd57e9ca9f808", val);
        }

        [Fact]
        public void Hash_String_To_Big_Int()
        {
            var actualBiggie = Md5Hash.HashStringToBigNasty("47bce5c74f589f4867dbd57e9ca9f808");
            var expectedBiggie = "95355999972893604581396806948474189832";
            Assert.Equal(expectedBiggie, actualBiggie.ToString());
        }
    }
}
