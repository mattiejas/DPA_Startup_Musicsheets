using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    public class SaveToPdfCommand : ICommand
    {
        private string _lilypondText;

        public SaveToPdfCommand(string lilypondText)
        {
            _lilypondText = lilypondText;
        }

        public void Execute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var tmp = $"{Path.ChangeExtension(saveFileDialog.FileName, null)}-tmp.ly";

                using (StreamWriter outputFile = new StreamWriter(tmp))
                {
                    outputFile.Write(_lilypondText);
                    outputFile.Close();
                }

                string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
                string sourceFolder = Path.GetDirectoryName(tmp);
                string sourceFileName = Path.GetFileNameWithoutExtension(tmp);
                string targetFolder = Path.GetDirectoryName(saveFileDialog.FileName);
                string targetFileName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                var process = new Process
                {
                    StartInfo = {
                        WorkingDirectory = sourceFolder,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                        FileName = lilypondLocation
                    }
                };

                process.Start();
                while (!process.HasExited)
                {
                    // Wait for exit
                }

                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
                {
                    File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                    File.Delete(tmp);
                }
            }
        }
    }
}
