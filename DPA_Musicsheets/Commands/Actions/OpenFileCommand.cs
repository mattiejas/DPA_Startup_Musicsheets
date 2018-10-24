using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    public class OpenFileCommand : ICommand
    {
        Action<string> _callback;

        public OpenFileCommand(Action<string> callback)
        {
            _callback = callback;
        }

        public void Execute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                _callback(openFileDialog.FileName);
            }
        }
    }
}
