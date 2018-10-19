using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public abstract class AbstractState
    {
        public Context Context { get; set; }

        public AbstractState(Context context)
        {
            Context = context;
        }

        public abstract void Handle();
    }
}
