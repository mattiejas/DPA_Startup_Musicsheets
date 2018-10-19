using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Strategies
{
    public class LilypondLoadStrategy : ILoadStrategy<string>
    {
        private readonly IViewManagerPool _pool;

        public LilypondLoadStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public void Load(string input)
        {
            var score = new LilypondScoreBuilder(input).Build();

            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }
        }
    }
}
