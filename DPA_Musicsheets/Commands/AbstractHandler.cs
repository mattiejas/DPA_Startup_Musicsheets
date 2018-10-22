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
        protected List<Key> _shortcut;
        protected Invoker _invoker;

        public AbstractHandler(Invoker invoker, List<Key> shortcut)
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
            if (_next != null)
            {
                return _next.Handle(request);
            }
            else
            {
                return null;
            }
        }

        public bool AreEqual(IList first, IList second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            for (var elementCounter = 0; elementCounter < first.Count; elementCounter++)
            {
                if (!first[elementCounter].Equals(second[elementCounter]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
