using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimhashLib
{
    public static class Shingling
    {
        public static List<string> Slide(string content, int width = 4)
        {
            var listOfShingles = new List<string>();
            for (var i = 0; i < (content.Length + 1 - width); i++)
            {
                var piece = content.Substring(i, width);
                listOfShingles.Add(piece);
            }
            return listOfShingles;
        }
        
        public static string Scrub(string content)
        {
            var matches = Regex.Matches(content, @"[\w\u4e00-\u9fcc]+");
            var ans = "";
            foreach (Match match in matches)
            {
                ans += match.Value;
            }

            return ans;
        }

        public static List<string> Tokenize(string content, int width = 4)
        {
            content = content.ToLower();
            content = Scrub(content);
            return Slide(content, width);
        }
    }
}
