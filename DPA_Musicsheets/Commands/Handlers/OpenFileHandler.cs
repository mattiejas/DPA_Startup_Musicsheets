using DPA_Musicsheets.Commands.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands.Handlers
{
    public class OpenFileHandler : AbstractHandler
    {
        private Action<string> _callback;

        public OpenFileHandler(Invoker invoker, Action<string> callback) : base(invoker)
        {
            _callback = callback;
        }

        public override Request Handle(Request request)
        {
            if ((request.PressedKeys.Contains(Key.LeftCtrl) || request.PressedKeys.Contains(Key.RightCtrl)) &&
                request.PressedKeys.Contains(Key.O))
            {
                var command = new OpenFileCommand(_callback);
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }

            return base.Handle(request);
        }
    }
}
