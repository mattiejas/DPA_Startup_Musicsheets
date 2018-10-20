using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands
{
    public class Request
    {
        public List<Key> PressedKeys { get; set; }
        public List<Key> Shortcut { get; set; }
    }
}
