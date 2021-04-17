using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimhashLib;

namespace SimhashTests
{
    [TestClass]
    public class TestShingling
    {

        [TestMethod]
        public void Slide()
        {
            var pieces = Shingling.Slide("aaabbb", width: 4);
            //aaab, aabb, abbb
            Assert.AreEqual(3, pieces.Count);
        }

        [TestMethod]
        public void Tokenize_Width_Default()
        {
            var pieces = Shingling.Tokenize("aaabbb");
            //aaab, aabb, abbb
            Assert.AreEqual(3, pieces.Count);
        }
        
        [TestMethod]
        public void Tokenize_Width_Three()
        {
            var pieces = Shingling.Tokenize("This is a test for really cool content. yeah! =)", width: 3);
            //thi, his, isi, sis, isa .. etc....
            Assert.AreEqual(33, pieces.Count);
        }
        
        [TestMethod]
        public void Clean()
        {
            var cleaned = Shingling.Scrub("aaa bbb test test testing. happy time =-).");
            Assert.AreEqual("aaabbbtesttesttestinghappytime", cleaned);
        }
    }
}
