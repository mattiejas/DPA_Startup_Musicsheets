using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    public class SaveToLilypondCommand : ICommand
    {
        private string _lilypondText;

        public SaveToLilypondCommand(string lilypondText)
        {
            _lilypondText = lilypondText;
        }

        public void Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly" };
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter outputFile = new StreamWriter(saveFileDialog.FileName))
                {
                    outputFile.Write(_lilypondText);
                    outputFile.Close();
                }
            }
        }
    }
}
