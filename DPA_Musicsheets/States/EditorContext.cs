using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public class EditorContext
    {
        public readonly IViewManagerPool Pool;
        public State CurrentState { get; private set; }

        public List<string> History { get; set; }
        public string CurrentEditorContent { get; set; }

        public EditorContext(IViewManagerPool pool)
        {
            Pool = pool;
            CurrentState = new TypingState(this);
            History = new List<string>();
        }

        public void SetState(State state)
        {
            CurrentState = state;
        }

        public void AddMemento(string input)
        {
            History.Add(input);
        }
    }
}
