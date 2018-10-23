using DPA_Musicsheets.Commands.Actions;
using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Interfaces;

namespace DPA_Musicsheets.Commands.Handlers
{
    class InsertHandler : AbstractHandler
    {
        private Insertable<string> _client;
        private string _value;

        public InsertHandler(Invoker invoker, Shortcut shortcut, Insertable<string> client, string value) : base(invoker, shortcut)
        {
            _client = client;
            _value = value;
        }

        public override Request Handle(Request request)
        {
            if (!request.Shortcut.Contains(_shortcut)) return base.Handle(request);

            var command = new InsertCommand(_client, _value);
            _invoker.SetCommand(command);
            _invoker.ExecuteCommand();

            request.Shortcut.Clear();
            return request;
        }
    }
}
