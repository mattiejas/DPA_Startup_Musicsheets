using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    class AddClefCommand : ICommand
    {
        private EditorContext _editorContext;
        private int _index;

        public AddClefCommand(EditorContext editorContext, int selectionIndex)
        {
            _editorContext = editorContext;
            _index = selectionIndex;
        }

        public void Execute()
        {
            var result = _editorContext.CurrentEditorContent.Substring(0, _index);
            result += "\n\\clef treble\n";
            result += _editorContext.CurrentEditorContent.Substring(_index);

            _editorContext.CurrentEditorContent = result;
        }
    }
}
