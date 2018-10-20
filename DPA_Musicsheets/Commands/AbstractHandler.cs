using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _next;
        protected Invoker _invoker;

        public AbstractHandler(Invoker invoker)
        {
            _invoker = invoker;
        }

        public IHandler SetNext(IHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual Request Handle(Request request)
        {
            if (_next != null)
            {
                return _next.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}
