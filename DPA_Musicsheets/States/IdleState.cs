using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DPA_Musicsheets.States
{
    public class IdleState : AbstractState
    {
        private Timer _timer;

        public IdleState(Context context) : base(context)
        {
            _timer = new Timer
            {
                Interval = 2000,
                AutoReset = true,
                Enabled = true,
            };
            _timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Context.SetState(new GeneratingState(Context));
            Context.CurrentState.Handle();
        }

        public override void Handle()
        {
            _timer.Dispose();
            Context.SetState(new TypingState(Context));
        }
    }
}
