using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public class Context
    {
        private Dictionary<string, string> keywords;
        private Context inner { get; set; }
        private static IList<string> reservedKeywords = new List<string> {
            "\\relative",
            "\\clef",
            "\\tempo",
            "\\time",
            "\\repeat",
            "\\alternative"
        };

        /*
         * \relative c {
         *
         * { } }
         *
         */

        public Context(string[] input)
        {
            keywords = new Dictionary<string, string>();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("\\") && reservedKeywords.Contains(input[i]))
                {
                    keywords.Add(input[i], input[++i]);
                }

                /*
                if (input[i] == "{")
                {

                }
                */
            }

            Console.Write(keywords);
        }
    }
}
