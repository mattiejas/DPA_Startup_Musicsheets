using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public class Note : Token
    {
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
