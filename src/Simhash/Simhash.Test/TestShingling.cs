using System.Text;
using SimhashLib;
using Xunit;

namespace Simhash.Test
{
    public class TestShingling
    {
        [Fact]
        public void Slide()
        {
            var pieces = Shingling.Slide("aaabbb", width: 4);
            //aaab, aabb, abbb
            Assert.Equal(3, pieces.Count);
        }

        [Fact]
        public void Tokenize_Width_Default()
        {
            var pieces = Shingling.Tokenize("aaabbb", new StringBuilder());
            //aaab, aabb, abbb
            Assert.Equal(3, pieces.Count);
        }
        
        [Fact]
        public void Tokenize_Width_Three()
        {
            var pieces = Shingling.Tokenize("This is a test for really cool content. yeah! =)", new StringBuilder(), width: 3);
            //thi, his, isi, sis, isa .. etc....
            Assert.Equal(33, pieces.Count);
        }
        
        [Fact]
        public void Clean()
        {
            var cleaned = Shingling.Scrub("aaa bbb test test testing. happy time =-).", new StringBuilder());
            Assert.Equal("aaabbbtesttesttestinghappytime", cleaned.ToString());
        }
    }
}
