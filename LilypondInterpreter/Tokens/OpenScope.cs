using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter.Tokens
{
    public class OpenScope : Token
    {
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
