using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public class Rest : Token
    {
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
