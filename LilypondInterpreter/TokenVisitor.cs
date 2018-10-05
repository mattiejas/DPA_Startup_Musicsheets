using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
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
        private readonly TokenScoreBuilder _builder;

        // TODO: IScoreBuilder is useless here, we need the methods from TokenScoreBuilder
        public TokenVisitor(TokenScoreBuilder builder)
        {
            _builder = builder;
        }

        public void Visit(Note note)
        {
            _builder.AddNote(note);
        }

        public void Visit(Rest rest)
        {
            _builder.AddRest(rest);
        }

        public void Visit(Keyword keyword)
        {
            _builder.AddKeyword(keyword);
        }

        public void Visit(CloseScope closeScope)
        {
            _builder.CloseScope();
        }

        public void Visit(OpenScope openScope)
        {
            _builder.OpenNewScope();
        }
    }
}
