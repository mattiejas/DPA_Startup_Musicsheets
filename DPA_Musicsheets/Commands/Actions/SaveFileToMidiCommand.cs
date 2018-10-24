using DPA_Musicsheets.Builders.Midi;
using DPA_Musicsheets.Builders.Score;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands.Actions
{
    class SaveFileToMidiCommand : ICommand
    {
        private string _fileName;
        private string _lilypondText;

        public SaveFileToMidiCommand(string fileName, string lilypondText)
        {
            _fileName = fileName;
            _lilypondText = lilypondText;
        }

        public void Execute()
        {
            var score = new LilypondScoreBuilder(_lilypondText).Build();
            var sequence = new SequenceBuilder(score).Build();
            sequence.Save(_fileName);
        }
    }
}
