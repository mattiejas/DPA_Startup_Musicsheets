using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter.Tokens
{
    public class Repeat : Token
    {
        public int Times { get; set; }
        public List<Token> Inner { get; set; }

        public override void Accept(ITokenVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
