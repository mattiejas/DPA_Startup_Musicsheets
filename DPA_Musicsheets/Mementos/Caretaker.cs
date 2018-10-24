using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Mementos
{
    public class Caretaker
    {
        private List<IMemento> _undos = new List<IMemento>();
        private List<IMemento> _redos = new List<IMemento>();
        private IOrginator _originator = null;

        public Caretaker(IOrginator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            this._undos.Add(this._originator.Save());
        }

        public void Redo()
        {
            if (!this.IsRedoable())
            {
                return;
            }

            var memento = _redos.Last();
            _undos.Add(memento);
            _redos.Remove(memento);

            Debug.WriteLine("Caretaker: Restoring state to: " + memento + "\n");

            try
            {
                this._originator.Restore(memento);
            }
            catch (InvalidMementoException /* exception */)
            {
                this.Undo();
            }

        }

        public void FlushRedos()
        {
            _redos.Clear();
        }

        public void Undo()
        {
            if (!this.IsUndoable())
            {
                return;
            }

            var memento = _undos.Last();
            _redos.Add(memento);
            _undos.Remove(memento);

            Debug.WriteLine("Caretaker: Restoring state to: " + _undos.Last().GetName() + "\n");

            try
            {
                this._originator.Restore(_undos.Last());
            }
            catch (InvalidMementoException /* exception */)
            {
                this.Undo();
            }
        }

        public bool IsUndoable()
        {
            return _undos.Count > 1;
        }

        public bool IsRedoable()
        {
            return _redos.Count > 0;
        }

        public IMemento Peek()
        {
            return _undos.Last();
        }

        public void ShowHistory()
        {
            foreach (var memento in _undos)
            {
                Debug.WriteLine(memento.GetName());
            }
        }
    }
}
