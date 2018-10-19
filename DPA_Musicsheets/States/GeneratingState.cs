using DPA_Musicsheets.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.States
{
    class GeneratingState : AbstractState
    {
        public GeneratingState(Context context) : base(context)
        {
        }

        public override void Handle()
        {
            if (Context.Mementos.Last() != Context.CurrentEditorContent)
            {
                try
                {
                    new LilypondLoadStrategy(Context.Pool).Load(Context.CurrentEditorContent);
                    Context.AddMemento(Context.CurrentEditorContent);
                } catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            Context.SetState(new IdleState(Context));
        }
    }
}
