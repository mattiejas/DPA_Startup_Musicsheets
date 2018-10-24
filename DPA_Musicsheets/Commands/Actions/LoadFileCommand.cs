using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    public class LoadFileCommand : ICommand
    {
        private MusicLoader _musicLoader;
        private string _fileName;

        public LoadFileCommand(MusicLoader musicLoader, string fileName)
        {
            _musicLoader = musicLoader;
            _fileName = fileName;
        }

        public void Execute()
        {
            _musicLoader.OpenFile(_fileName);
        }
    }
}
