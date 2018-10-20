using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Mementos
{
    class ConcreteMemento : IMemento
    {
        private string _state;
        private Guid _name;
        private DateTime _date;

        public ConcreteMemento(string state)
        {
            _state = state;
            _date = DateTime.Now;
            _name = Guid.NewGuid();
        }

        public string GetName()
        {
            return _date + " // " + _name;
        }

        public DateTime GetDate()
        {
            return _date;
        }

        public string GetState()
        {
            return _state;
        }
    }
}
