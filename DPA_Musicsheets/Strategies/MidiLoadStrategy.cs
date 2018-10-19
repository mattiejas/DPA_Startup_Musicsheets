using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Strategies
{
    public class MidiLoadStrategy : ILoadStrategy<Sequence>
    {
        private readonly IViewManagerPool _pool;

        public MidiLoadStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public void Load(Sequence input)
        {
            var score = new MidiScoreBuilder(input).Build();

            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }
        }
    }
}
