using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Strategies
{
    public interface ISaveStrategy
    {
        void Handle(string fileName, EditorContext editorContext);
    }
}
