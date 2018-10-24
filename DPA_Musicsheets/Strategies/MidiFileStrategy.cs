using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Strategies
{
    class MidiFileStrategy : IFileStrategy
    {
        private readonly ILoadStrategy<Sequence> _loader;

        public MidiFileStrategy(ILoadStrategy<Sequence> loader)
        {
            _loader = loader;
        }

        public void Handle(string filename)
        {
            var sequence = new Sequence();
            sequence.Load(filename);

            _loader.Load(sequence);
            _loader.Apply();
        }
    }
}
