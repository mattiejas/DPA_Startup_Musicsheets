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

        public SaveToPdfHandler(Invoker invoker, Shortcut shortcut, EditorContext editorContext) : base(invoker, shortcut)
        {
            _editorContext = editorContext;
        }

        public override Request Handle(Request request)
        {
            if (!request.Shortcut.Contains(_shortcut)) return base.Handle(request);

            var command = new SaveToPdfCommand(_editorContext.CurrentEditorContent);
            _invoker.SetCommand(command);
            _invoker.ExecuteCommand();

            request.Shortcut.Clear();
            return request;
        }
    }
}
