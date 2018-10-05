using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Strategies
{
    class MidiFileStrategy : IFileStrategy
    {
        private readonly IViewManagerPool _pool;
        private IScoreBuilder _builder;

        public MidiFileStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public void Handle(string filename)
        {
            var sequence = new Sequence();
            sequence.Load(filename);

            // TODO: load lilypond text 
            // this.LilypondText = LoadMidiIntoLilypond(MidiSequence);
            // this.LilypondViewModel.LilypondTextLoaded(this.LilypondText);

            _builder = new MidiScoreBuilder(sequence);
            var score = _builder.Build();
            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }
        }
    }
}
