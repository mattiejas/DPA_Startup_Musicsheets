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

            foreach (var symbolGroup in score.SymbolGroups)
            {
                _builder.AddSymbolGroup(symbolGroup);
            }

            _view.Load(_builder.Build());
        }

        public void RegisterViewModel(IView<string> view)
        {
            _view = view;
        }
    }
}
