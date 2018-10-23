using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands
{
    public abstract class AbstractHandler : IHandler
    {
        protected IHandler _next;
        protected Shortcut _shortcut;
        protected Invoker _invoker;

        protected AbstractHandler(Invoker invoker, Shortcut shortcut)
        {
            _invoker = invoker;
            _shortcut = shortcut;
        }

        public IHandler SetNext(IHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual Request Handle(Request request)
        {
            return _next?.Handle(request);
        }
    }
}
