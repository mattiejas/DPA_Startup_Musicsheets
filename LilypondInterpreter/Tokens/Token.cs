using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public abstract class Token
    {
        public string Value { get; set; }

        public abstract void Accept(ITokenVisitor visitor);
    }
}
