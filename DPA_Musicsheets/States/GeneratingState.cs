using DPA_Musicsheets.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DPA_Musicsheets.Managers.View;

namespace DPA_Musicsheets.States
{
    public class GeneratingState : State
    {
        public GeneratingState(EditorContext context) : base(context)
        {
        }

        public override void Handle()
        {
            if (Context.History.Last() != Context.CurrentEditorContent)
            {
                var loader = new LilypondLoadStrategy(Context.Pool);
                var success = loader.Load(Context.CurrentEditorContent);
                if (success)
                {
                    loader.Apply(vm => !(vm is LilypondViewManager));
                    Context.AddMemento(Context.CurrentEditorContent);
                }
            }

            Context.SetState(new IdleState(Context));
        }
    }
}