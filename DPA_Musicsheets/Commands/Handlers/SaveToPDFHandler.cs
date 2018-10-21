using DPA_Musicsheets.Commands.Actions;
using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands.Handlers
{
    class SaveToPdfHandler : AbstractHandler
    {
        private EditorContext _editorContext;

        public SaveToPdfHandler(Invoker invoker, EditorContext editorContext) : base(invoker)
        {
            _editorContext = editorContext;
        }

        public override Request Handle(Request request)
        {
            if ((request.PressedKeys.Contains(Key.LeftCtrl) || request.PressedKeys.Contains(Key.RightCtrl)) &&
                request.PressedKeys.Contains(Key.P) && request.PressedKeys.Contains(Key.S))
            {
                var command = new SaveToPdfCommand(_editorContext.CurrentEditorContent);
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }
            return base.Handle(request);
        }
    }
}
