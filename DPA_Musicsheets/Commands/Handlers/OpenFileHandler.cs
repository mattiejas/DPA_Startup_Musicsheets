﻿using DPA_Musicsheets.Commands.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands.Handlers
{
    public class OpenFileHandler : AbstractHandler
    {
        private readonly Action<string> _callback;

        public OpenFileHandler(Invoker invoker, List<Key> shortcut, Action<string> callback) : base(invoker, shortcut)
        {
            _callback = callback;
        }

        public override Request Handle(Request request)
        {
            if (this.AreEqual(request.PressedKeys, _shortcut))
            {
                request.PressedKeys.Clear();

                var command = new OpenFileCommand(_callback);
                _invoker.SetCommand(command);
                _invoker.ExecuteCommand();
                return request;
            }

            return base.Handle(request);
        }
    }
}
