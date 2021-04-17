using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SimhashLib
{
    public static class Shingling
    {
        private static readonly Regex ScrubRegex = new Regex(@"[\w\u4e00-\u9fcc]+", RegexOptions.Compiled);
        
        public static List<string> Slide(string content, int width = 4)
        {
            var listOfShingles = width > 0 ? new List<string>(content.Length / width + 1) : new List<string>();
            for (var i = 0; i < (content.Length + 1 - width); i++)
            {
                var piece = content.Substring(i, width);
                listOfShingles.Add(piece);
            }
            return listOfShingles;
        }
        
        public static StringBuilder Scrub(string content, StringBuilder builder)
        {
            var matches = ScrubRegex.Matches(content);
            foreach (Match match in matches)
                builder.Append(match.Value);

            return builder;
        }

        public static List<string> Tokenize(string content, StringBuilder builder, int width = 4)
        {
            content = content.ToLower();
            Scrub(content, builder);
            return Slide(builder.ToString(), width);
        }
    }
}
