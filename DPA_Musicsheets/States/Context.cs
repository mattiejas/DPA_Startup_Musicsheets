using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public class Context
    {
        public readonly IViewManagerPool Pool;
        public AbstractState CurrentState { get; private set; }

        public Context(IViewManagerPool pool)
        {
            Pool = pool;
            CurrentState = new TypingState(this);
        }

        public void SetState(AbstractState state)
        {
            CurrentState = state;
        }

        public void Request()
        {
            CurrentState.Handle();
        }
    }
}
