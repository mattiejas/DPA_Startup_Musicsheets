using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    class SaveFileToLilypondCommand : ICommand
    {
        private string _fileName;
        private string _lilypondText;

        public SaveFileToLilypondCommand(string fileName, string lilypondText)
        {
            _fileName = fileName;
            _lilypondText = lilypondText;
        }

        public void Execute()
        {
            using (StreamWriter outputFile = new StreamWriter(_fileName))
            {
                outputFile.Write(_lilypondText);
                outputFile.Close();
            }
        }
    }
}
