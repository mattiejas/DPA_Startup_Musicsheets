using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace LilypondInterpreter
{
    public static class Interpreter
    {
        public static Score Interpret(List<Token> tokens)
        {
            var builder = new TokenScoreBuilder();

            var visitor = new TokenVisitor(builder);
            foreach (var token in tokens)
            {
                token.Accept(visitor);
            }

            return builder.Build();
        }
    }
}
