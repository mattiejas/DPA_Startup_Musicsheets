using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Interfaces;
using Common.Models;
using DPA_Musicsheets.Builders.View;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Managers.View
{
    public class LilypondViewManager : IViewManager
    {
        private IView<string> _view;
        private IViewBuilder<string> _builder;

        public LilypondViewManager()
        {
            _builder = new LilypondViewBuilder();
        }

        public void Load(Score score)
        {
            if (_view == null) throw new ViewModelNotFoundException();

            _builder.Reset(); // reset builder so symbols don't stack

            _builder.AddClef(score.Clef);
            _builder.AddTimeSignature(score.SymbolGroups[0].Meter);

            foreach (var symbolGroup in score.SymbolGroups)
            {
                _builder.AddTimeSignature(symbolGroup.Meter);
                _builder.AddTempo(symbolGroup.Tempo);

                foreach (var symbol in symbolGroup.Symbols)
                {
                    if (symbol is Note note)
                    {
                        _builder.AddNote(note);
                    }

                    if (symbol is Rest rest)
                    {
                        _builder.AddRest(rest);
                    }
                }
            }

            _view.Load(_builder.Build());
        }

        public void RegisterViewModel(IView<string> view)
        {
            _view = view;
        }
    }
}
