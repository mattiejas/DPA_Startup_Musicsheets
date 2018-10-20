using Common.Interfaces;
using DPA_Musicsheets.Mementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    public class EditorContext : IOrginator
    {
        public readonly IViewManagerPool Pool;
        public State CurrentState { get; private set; }
        public bool IsRestored { get; set; }
        public string CurrentEditorContent { get; set; }
        public Caretaker Caretaker { get; set; }

        public EditorContext(IViewManagerPool pool)
        {
            IsRestored = false;
            Pool = pool;
            CurrentState = new TypingState(this);
            Caretaker = new Caretaker(this);
        }

        public void SetState(State state)
        {
            CurrentState = state;
        }

        public IMemento Save()
        {
            return new ConcreteMemento(CurrentEditorContent);
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            this.IsRestored = true;
            this.CurrentEditorContent = memento.GetState();
        }
    }
}
