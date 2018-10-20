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
        private List<IMemento> _mementos = new List<IMemento>();
        private IOrginator _originator = null;

        public Caretaker(IOrginator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            Debug.WriteLine("Creating backup");
            this._mementos.Add(this._originator.Save());
            this.ShowHistory();
        }

        public void Undo()
        {
            if (!this.IsUndoable())
            {
                return;
            }

            _mementos.Remove(_mementos.Last());

            Debug.WriteLine("Caretaker: Restoring state to: " + _mementos.Last().GetName() + "\n");

            try
            {
                this._originator.Restore(_mementos.Last());
            }
            catch (Exception ex)
            {
                this.Undo();
            }
        }

        public bool IsUndoable()
        {
            return _mementos.Count > 1;
        }

        public IMemento Peek()
        {
            return _mementos.Last();
        }

        public void ShowHistory()
        {
            foreach (var memento in _mementos)
            {
                Debug.WriteLine(memento.GetName());
            }
        }
    }
}
