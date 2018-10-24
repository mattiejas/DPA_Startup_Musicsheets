using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace DPA_Musicsheets.Strategies
{
    public class MidiLoadStrategy : ILoadStrategy<Sequence>
    {
        private readonly IViewManagerPool _pool;
        private Score _score;

        public MidiLoadStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public bool Load(Sequence input)
        {
            var score = new MidiScoreBuilder(input).Build();
            if (score == null) return false;

            _score = score;
            return true;
        }

        public void Apply()
        {
            foreach (var viewManager in _pool)
            {
                viewManager.Load(_score);
            }
        }

        public void Apply(Func<IViewManager, bool> match)
        {
            foreach (var viewManager in _pool.Where(match))
            {
                viewManager.Load(_score);
            }
        }
    }
}
