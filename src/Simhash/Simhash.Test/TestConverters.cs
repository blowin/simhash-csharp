using System;
using SimhashLib;
using Xunit;

namespace Simhash.Test
{
    public class TestConverters
    {

        [Fact]
        public void Ulong_To_Long_Back_To_Ulong_Strings()
        {
            var theUlong = 18446744073709551615;
            var stheUlong = Converters.ConvertUlongToBin(theUlong);
            var cLong = Convert.ToInt64(stheUlong, 2);
            //save to mongo or other db using long (as ulong aren't!)

            //retrieve from db and then get back to ulong
            var sLong = Convert.ToString(cLong, 2);
            var fromDb = Convert.ToUInt64(sLong, 2);
            var thedbUlong = Converters.ConvertUlongToBin(fromDb);

            Assert.Equal(stheUlong, thedbUlong);
        }

        [Fact]
        public void Ulong_To_Long_Back_To_Ulong_Native()
        {
            var theUlong = 18446744073709551615;
            var cLong = Converters.ConvertUlongToLong(theUlong);
            //save to mongo/sql/etc (ulong's not accepted as valid payment at all locations)

            //retrieve from db and then get back to ulong
            var sLong = Convert.ToString(cLong, 2);
            var fromDb = Convert.ToUInt64(sLong, 2);
           
            Assert.Equal(theUlong, fromDb);
        }

        [Fact]
        public void Ulong_To_Binary()
        {
            ulong theUlong = 8637903533912358349;
            var stheUlong = Converters.ConvertulongToString(theUlong);
            var expSimHash = "111011111011111111111110111111110011110111011111111110111001101";
            Assert.Equal(expSimHash, stheUlong);
        }
    }
}
