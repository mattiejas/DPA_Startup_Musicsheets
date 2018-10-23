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
    class AddClefHandler : AbstractHandler
    {
        private EditorContext _editorContext;
        private int _selectionIndex;

        public AddClefHandler(Invoker invoker, List<Key> shortcut, EditorContext editorContext, int selectionIndex) : base(invoker, shortcut)
        {
            _editorContext = editorContext;
            _selectionIndex = selectionIndex;
        }

        public override Request Handle(Request request)
        {
            if (AreEqual(request.PressedKeys, _shortcut))
            {
                request.PressedKeys.Clear();

                var command = new AddClefCommand(_editorContext, _selectionIndex);
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }
            return base.Handle(request);
        }
    }
}
