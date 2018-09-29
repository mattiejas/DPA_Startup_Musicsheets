using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Strategies
{
    class MidiFileStrategy : IFileStrategy
    {
        private readonly IViewManagerPool _pool;
        private readonly IScoreBuilder<Sequence> _builder;

        public MidiFileStrategy(IViewManagerPool pool)
        {
            _pool = pool;
            _builder = new MidiScoreBuilder();
        }

        public void Handle(string filename)
        {
            var sequence = new Sequence();
            sequence.Load(filename);

            // TODO: load lilypond text 
            // this.LilypondText = LoadMidiIntoLilypond(MidiSequence);
            // this.LilypondViewModel.LilypondTextLoaded(this.LilypondText);

            var score = _builder.Build(sequence);
            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }
        }
    }
}
