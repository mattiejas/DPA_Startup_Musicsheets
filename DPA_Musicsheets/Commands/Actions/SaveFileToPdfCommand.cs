using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    class SaveFileToPdfCommand : ICommand
    {
        private string _fileName;
        private string _lilypondText;

        public SaveFileToPdfCommand(string fileName, string lilypondText)
        {
            _fileName = fileName;
            _lilypondText = lilypondText;
        }

        public void Execute()
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(_fileName);
            string tmpFileName = $"{_fileName}-tmp.ly";

            var lilypondCommand = new SaveFileToLilypondCommand(tmpFileName, _lilypondText);
            lilypondCommand.Execute();

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(_fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(_fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }
            if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }
    }
}
