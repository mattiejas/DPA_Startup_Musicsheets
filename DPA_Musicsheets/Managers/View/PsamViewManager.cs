using System;
using System.Collections.Generic;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.ViewModels;
using Common.Interfaces;
using Common.Models;
using Common.Exceptions;
using Note = Common.Models.Note;
using Rest = Common.Models.Rest;
using DPA_Musicsheets.Builders.View;
using PSAMControlLibrary;

namespace DPA_Musicsheets.Managers.View
{
    public class PsamViewManager : IViewManager
    {
        private readonly PsamViewBuilder _builder;
        private IView<IList<MusicalSymbol>> _view;

        public PsamViewManager()
        {
            _builder = new PsamViewBuilder();
        }

        public void RegisterViewModel(IView<IList<MusicalSymbol>> view)
        {
            _view = view;
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
    }
}
