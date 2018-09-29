using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
    }

    public enum TokenType
    {
        Clef,
        TimeSignature,
        Note,
        Rest
    }
}
