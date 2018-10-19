using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace DPA_Musicsheets.States
{
    public class IdleState : State
    {
        private DispatcherTimer _timer;

        public IdleState(EditorContext context) : base(context)
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(OnTimedEvent);
            _timer.Interval = new TimeSpan(0, 0, 3);
            _timer.Start();
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            Context.SetState(new GeneratingState(Context));
            Context.CurrentState.Handle();

            _timer.Stop();
        }

        public override void Handle()
        {
            _timer.Stop();
            Context.SetState(new TypingState(Context));
        }
    }
}
