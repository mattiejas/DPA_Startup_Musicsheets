using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public static List<Token> Tokenize(string[] entries)
        {
            var tokens = new List<Token>();

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].StartsWith("\\")) // keyword
                {
                    var keyword = GetKeyword(entries[i], entries, ref i);
                    tokens.Add(keyword);
                }
                else if (entries[i] == "{" || entries[i] == "}") // open scope
                {
                    tokens.Add(GetScope(entries[i]));
                }
                else if (entries[i] == "|")
                {
                    // do nothing
                }
                else
                {
                    tokens.Add(GetRestOrNote(entries[i]));
                }
            }

            return tokens;
        }

        public static List<Token> Tokenize(string input)
        {
            var entries = input.Split(new[] {' ', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            return Tokenize(entries);
        }

        private static Token GetKeyword(string keyword, string[] values, ref int i)
        {
            if (!ReservedKeywords.ContainsKey(keyword)) return null;

            var kw = ReservedKeywords[keyword];
            switch (kw)
            {
                case Keywords.Repeat:
                    var value = values[++i];
                    var count = values[++i];

                    // skip count
                    i += 1;
                    var j = i;

                    var openScopes = 0;
                    for (; j < values.Length; j++)
                    {
                        if (values[j] == "{") // open scope
                        {
                            openScopes++;
                        }
                        else if (values[j] == "}")
                        {
                            openScopes--;
                            if (openScopes <= 0)
                            {
                                break;
                            }
                        }
                    }

                    var repeat = new Repeat
                    {
                        Value = value,
                        Inner = Tokenize(values.Skip(i + 1).Take(j - (i + 1)).ToArray()),
                        Times = int.Parse(count),
                    };
                    i = j;
                    var alternatives = new List<List<Token>>();

                    if (values[j + 1] == "\\alternative")
                    {
                        j += 3; // skip first open bracket
                        openScopes = 1;

                        for (; j < values.Length; j++)
                        {
                            if (values[j] == "{") // open scope
                            {
                                openScopes++;
                                alternatives.Add(new List<Token>());
                            }
                            else if (values[j] == "}")
                            {
                                openScopes--;
                                if (openScopes <= 0)
                                {
                                    break;
                                }
                            } else
                            {
                                alternatives.Last().AddRange(Tokenize(values[j]));
                            }
                        }
                    }

                    i = j;
                    repeat.Alternatives = alternatives;
                    return repeat;
                default:
                    return new Keyword
                    {
                        Type = ReservedKeywords[keyword],
                        Value = values[++i]
                    };
            }
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