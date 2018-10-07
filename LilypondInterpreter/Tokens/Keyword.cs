using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public class Keyword : Token
    {
        public Keywords Type { get; set; }

        public override void Accept(ITokenVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
