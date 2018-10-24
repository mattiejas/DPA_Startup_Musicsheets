using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public abstract class State
    {
        public EditorContext Context { get; set; }

        protected State(EditorContext context)
        {
            Context = context;
        }

        public abstract void Handle();
    }
}
