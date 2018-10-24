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

            foreach (var symbolGroup in score.SymbolGroups)
            {
                _builder.AddSymbolGroup(symbolGroup);
            }

            _view.Load(_builder.Build());
        }
    }
}
