using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Definitions;
using Common.Exceptions;
using Common.Interfaces;
using Common.Models;
using DPA_Musicsheets.Builders.Midi;
using DPA_Musicsheets.ViewModels;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Managers.View
{
    public class MidiPlayerViewManager : IViewManager
    {
        private IView<Sequence> _view;

        public void RegisterViewModel(IView<Sequence> view)
        {
            _view = view;
        }

        public void Load(Score score)
        {
            if (_view == null) throw new ViewModelNotFoundException();
            var sequence = new SequenceBuilder(score).Build();
            _view.Load(sequence);
        }
    }
}
