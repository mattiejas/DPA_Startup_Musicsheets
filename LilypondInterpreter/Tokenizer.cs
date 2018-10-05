using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LilypondInterpreter.Tokens;

namespace LilypondInterpreter
{
    public static class Tokenizer
    {
        private static readonly IDictionary<string, Keywords> ReservedKeywords = new Dictionary<string, Keywords>
        {
            {"\\relative", Keywords.Relative},
            {"\\clef", Keywords.Clef},
            {"\\tempo", Keywords.Tempo},
            {"\\time", Keywords.TimeSignature},
            {"\\repeat", Keywords.Repeat},
            {"\\alternative", Keywords.Alternative}
        };

        public static List<Token> Tokenize(string input)
        {
            var entries = input.Split(new[] {' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new List<Token>();

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].StartsWith("\\")) // keyword
                {
                    tokens.Add(GetKeyword(entries[i], entries[++i]));
                }
                else if (entries[i] == "{" || entries[i] == "}") // open scope
                {
                    tokens.Add(GetScope(entries[i]));
                }
                else
                {
                    tokens.Add(GetRestOrNote(entries[i]));
                }
            }

            return tokens;
        }

        private static Token GetKeyword(string keyword, string value)
        {
            if (ReservedKeywords.ContainsKey(keyword)) // do we support this keyword
            {
                return new Keyword
                {
                    Type = ReservedKeywords[keyword],
                    Value = value,
                };
            }

            return null;
        }

        private static Token GetScope(string scope)
        {
            if (scope == "{") return new OpenScope();
            return new CloseScope();
        }

        private static Token GetRestOrNote(string token)
        {
            if (token.StartsWith("r"))
            {
                return new Rest {Value = token};
            }

            return new Note {Value = token};
        }
    }
}