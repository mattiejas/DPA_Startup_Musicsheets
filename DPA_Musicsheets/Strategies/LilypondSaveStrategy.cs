using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Commands.Actions;
using DPA_Musicsheets.States;

namespace DPA_Musicsheets.Strategies
{
    public class LilypondSaveStrategy : ISaveStrategy
    {
        public void Handle(string fileName, EditorContext editorContext)
        {
            var command = new SaveFileToLilypondCommand(fileName, editorContext.CurrentEditorContent);
            command.Execute();

            editorContext.SavedState = editorContext.CurrentEditorContent;
        }
    }
}
