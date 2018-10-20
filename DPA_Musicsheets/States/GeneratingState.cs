using DPA_Musicsheets.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DPA_Musicsheets.Managers.View;
using DPA_Musicsheets.Exceptions;

namespace DPA_Musicsheets.States
{
    public class GeneratingState : State
    {
        public GeneratingState(EditorContext context) : base(context)
        {
        }

        public override void Handle()
        {
            if (Context.IsRestored || Context.Caretaker.Peek().GetState() != Context.CurrentEditorContent)
            {
                try
                {
                    var loader = new LilypondLoadStrategy(Context.Pool);
                    var success = loader.Load(Context.CurrentEditorContent);
                    if (success)
                    {
                        if (Context.IsRestored)
                        {
                            loader.Apply();
                            Context.IsRestored = false;
                        }
                        else
                        {
                            loader.Apply(vm => !(vm is LilypondViewManager));
                            Context.Caretaker.FlushRedos();
                            Context.Caretaker.Backup();
                        }
                    }
                }
                catch (InvalidScoreException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            Context.SetState(new IdleState(Context));
        }
    }
}