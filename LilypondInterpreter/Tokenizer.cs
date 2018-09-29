using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LilypondInterpreter
{
    public static class Tokenizer
    {
        public static Context Tokenize(string input)
        {
            var tokens = input.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new Context(tokens);
        }
    }
}
