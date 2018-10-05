using System;
using System.Collections.Generic;
using System.Text;
using LilypondInterpreter.Tokens;

namespace LilypondInterpreter
{
    public interface ITokenVisitor
    {
        void Visit(Note note);
        void Visit(Rest rest);
        void Visit(CloseScope closeScope);
        void Visit(Keyword keyword);
        void Visit(OpenScope openScope);
    }

    public class TokenVisitor : ITokenVisitor
    {
        public void Visit(Note note)
        {
            throw new NotImplementedException();
        }

        public void Visit(Rest rest)
        {
            throw new NotImplementedException();
        }

        public void Visit(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public void Visit(CloseScope closeScope)
        {
            throw new NotImplementedException();
        }

        public void Visit(OpenScope openScope)
        {
            throw new NotImplementedException();
        }
    }
}
