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

        public List<string> Mementos { get; set; }
        public string CurrentEditorContent { get; set; }

        public Context(IViewManagerPool pool)
        {
            Pool = pool;
            CurrentState = new TypingState(this);
            Mementos = new List<string>();
        }

        public void SetState(AbstractState state)
        {
            CurrentState = state;
        }

        public void Request()
        {
            CurrentState.Handle();
        }

        public void AddMemento(string input)
        {
            Mementos.Add(input);
        }
    }
}
