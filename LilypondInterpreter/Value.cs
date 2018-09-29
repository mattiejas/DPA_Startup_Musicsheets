using Common.Models;
using System.Collections.Generic;

namespace LilypondInterpreter
{
    public class Value : IToken
    {
        private readonly string _v;

        public Value(string v)
        {
            _v = v;
        }

        public List<Token> Interpret()
        {
            return new List<Token> { new Token { Type = TokenType.Note, Value = _v } };
        }
    }
}