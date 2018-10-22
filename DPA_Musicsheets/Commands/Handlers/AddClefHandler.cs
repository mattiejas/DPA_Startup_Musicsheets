using DPA_Musicsheets.Commands.Actions;
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
        private string _lilypondText;

        public AddClefHandler(Invoker invoker, List<Key> shortcut, string lilypondText) : base(invoker, shortcut)
        {
            _lilypondText = lilypondText;
        }

        public override Request Handle(Request request)
        {
            if (this.AreEqual(request.PressedKeys, _shortcut))
            {
                request.PressedKeys.Clear();

                var command = new AddClefCommand();
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }
            return base.Handle(request);
        }
    }
}
