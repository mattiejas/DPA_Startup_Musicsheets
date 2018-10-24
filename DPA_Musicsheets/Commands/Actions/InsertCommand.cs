using DPA_Musicsheets.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace DPA_Musicsheets.Commands.Actions
{
    class InsertCommand : ICommand
    {
        private Insertable<string> _client;
        private string _value;

        public InsertCommand(Insertable<string> client, string value)
        {
            _client = client;
            _value = value;
        }

        public void Execute()
        {
            _client.Insert(_value);
        }
    }
}
