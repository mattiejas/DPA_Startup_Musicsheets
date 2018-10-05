using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LilypondInterpreter
{
    public static class Tokenizer
    {
        private static IDictionary<string, Keywords> reservedKeywords = new Dictionary<string, Keywords> {
            { "\\relative", Keywords.Relative },
            { "\\clef", Keywords.Clef },
            { "\\tempo", Keywords.Tempo },
            { "\\time", Keywords.TimeSignature },
            { "\\repeat", Keywords.Repeat },
            { "\\alternative", Keywords.Alternative }
        };

        public static List<Token> Tokenize(string input)
        {
            var entries = input.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new List<Token>();

            for (int i = 0; i < entries.Length; i++)
            { 
                if (entries[i].StartsWith("\\")) // keyword
                {
                    tokens.Add(GetKeyword(entries[i], entries[++i]));
                } else if (entries[i] == "{")
                {
                    tokens.Add(new Keyword { Type = Keywords.OpenScope, Value = entries[i] });
                } else if (entries[i].StartsWith("r"))
                {
                    tokens.Add(new Token { Type = TokenType.Rest, Value = entries[i] });
                } else
                {
                    tokens.Add(new Token { Type = TokenType.Note, Value = entries[i] });
                }
            }
            return tokens;
        }

        private static Token GetKeyword(string keyword, string value)
        {
            if (reservedKeywords.ContainsKey(keyword)) // do we support this keyword
            {
                return new Keyword
                {
                    Type = reservedKeywords[keyword],
                    Value = value,
                };
            }
            return null;
        }
    }
}
