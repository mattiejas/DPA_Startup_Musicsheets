using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public static class Interpreter
    {
        public static Score Interpret(List<Token> tokens)
        {
            var score = new Score();
            var visitor = new TokenVisitor();
            foreach (var token in tokens)
            {
                token.Accept(visitor);
            }
        }
    }
}
