using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LilypondInterpreter
{
    public class Scope : IToken
    {
        public Dictionary<string, string> Keywords { get; private set; }

        private string[] _input;

        public List<IToken> Tokens { get; private set; }

        private static IList<string> reservedKeywords = new List<string> {
            "\\relative",
            "\\clef",
            "\\tempo",
            "\\time",
            "\\repeat",
            "\\alternative"
        };

        public Scope(string[] input)
        {
            Keywords = new Dictionary<string, string>();
            Tokens = new List<IToken>();

            _input = input;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _input.Length; i++)
            {
                if (_input[i].StartsWith("\\") && reservedKeywords.Contains(_input[i]))
                {
                    Keywords.Add(_input[i], _input[++i]);
                } else if (_input[i] == "{")
                {
                    for (int j = _input.Length - 1; j >= i; j--)
                    {
                        if (_input[j] == "}")
                        {
                            Tokens.Add(new Scope(_input.Skip(i).Take(j - i).ToArray()));
                            i = j; // skip parsing inner scope
                            break;
                        }
                    }
                } else
                {
                    Tokens.Add(new Value(_input[i]));
                }
            }
        }

        public List<Token> Interpret()
        {
            var tokens = new List<Token>();

            foreach (var token in Tokens)
            {
                tokens.AddRange(token.Interpret());
            }

            return tokens;
        }
    }
}
