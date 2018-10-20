using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Mementos
{
    public interface IMemento
    {
        string GetName();
        string GetState();
        DateTime GetDate();
    }
}
