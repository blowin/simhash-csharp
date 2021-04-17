using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimhashLib;

namespace SimhashTests
{
    [TestClass]
    public class TestConverters
    {

        [TestMethod]
        public void Ulong_To_Long_Back_To_Ulong_Strings()
        {
            var theUlong = 18446744073709551615;
            var stheUlong = Converters.convert_ulong_to_bin(theUlong);
            var cLong = Convert.ToInt64(stheUlong, 2);
            //save to mongo or other db using long (as ulong aren't!)

            //retrieve from db and then get back to ulong
            var sLong = Convert.ToString(cLong, 2);
            var fromDb = Convert.ToUInt64(sLong, 2);
            var thedbUlong = Converters.convert_ulong_to_bin(fromDb);

            Assert.AreEqual(stheUlong, thedbUlong);
        }

        [TestMethod]
        public void Ulong_To_Long_Back_To_Ulong_Native()
        {
            var theUlong = 18446744073709551615;
            var cLong = Converters.ConvertUlongToLong(theUlong);
            //save to mongo/sql/etc (ulong's not accepted as valid payment at all locations)

            //retrieve from db and then get back to ulong
            var sLong = Convert.ToString(cLong, 2);
            var fromDb = Convert.ToUInt64(sLong, 2);
           
            Assert.AreEqual(theUlong, fromDb);
        }

        [TestMethod]
        public void Ulong_To_Binary()
        {
            ulong theUlong = 8637903533912358349;
            var stheUlong = Converters.ConvertulongToString(theUlong);
            var expSimHash = "111011111011111111111110111111110011110111011111111110111001101";
            Assert.AreEqual(expSimHash, stheUlong);
        }
    }
}
