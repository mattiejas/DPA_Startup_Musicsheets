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
            try
            {
                if (Context.History.Last() != Context.CurrentEditorContent)
                {
                    var loader = new LilypondLoadStrategy(Context.Pool);
                    var success = loader.Load(Context.CurrentEditorContent);
                    if (success)
                    {
                        loader.Apply();
                        Context.AddMemento(Context.CurrentEditorContent);
                    }
                }
            } catch(InvalidScoreException e)
            {
                Debug.WriteLine(e.Message);
            }

            Context.SetState(new IdleState(Context));
        }
    }
}