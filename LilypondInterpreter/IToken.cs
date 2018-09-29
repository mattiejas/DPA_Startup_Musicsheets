using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilypondInterpreter
{
    public interface IToken
    {
        List<Token> Interpret();
    }
}
