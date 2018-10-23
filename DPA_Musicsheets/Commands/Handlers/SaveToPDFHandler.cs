using DPA_Musicsheets.Commands.Actions;
using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands.Handlers
{
    class SaveToPdfHandler : AbstractHandler
    {
        private EditorContext _editorContext;

        public SaveToPdfHandler(Invoker invoker, List<Key> shortcut, EditorContext editorContext) : base(invoker, shortcut)
        {
            _editorContext = editorContext;
        }

        public override Request Handle(Request request)
        {
            if (AreEqual(request.PressedKeys, _shortcut))
            {
                request.PressedKeys.Clear();

                var command = new SaveToPdfCommand(_editorContext.CurrentEditorContent);
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }

            return base.Handle(request);
        }
    }
}
