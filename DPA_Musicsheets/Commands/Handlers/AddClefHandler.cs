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

        public AddClefHandler(Invoker invoker, string lilypondText) : base(invoker)
        {
            _lilypondText = lilypondText;
        }

        public override Request Handle(Request request)
        {
            if ((request.PressedKeys.Contains(Key.LeftAlt) || request.PressedKeys.Contains(Key.RightAlt)) &&
                request.PressedKeys.Contains(Key.C))
            {
                var command = new AddClefCommand();
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }
            return base.Handle(request);
        }
    }
}
