using Common.Interfaces;
using DPA_Musicsheets.Builders.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using DPA_Musicsheets.Exceptions;

namespace DPA_Musicsheets.Strategies
{
    public class LilypondLoadStrategy : ILoadStrategy<string>
    {
        private readonly IViewManagerPool _pool;
        private Score _score;

        public LilypondLoadStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public bool Load(string input)
        {
            try
            {
                var score = new LilypondScoreBuilder(input).Build();
                if (score == null) return false;

                _score = score;
                return true;
            }
            catch (Exception e)
            {
                throw new InvalidScoreException("Couldn't generate score", e);
            }
        }

        public void Apply()
        {
            try
            {
                foreach (var viewManager in _pool)
                {
                    viewManager.Load(_score);
                }
            }
            catch (Exception e)
            {
                throw new InvalidScoreException("Couldn't load score", e);
            }
        }

        public void Apply(Func<IViewManager, bool> match)
        {
            try
            {
                foreach (var viewManager in _pool.Where(match))
                {
                    viewManager.Load(_score);
                }
            }
            catch (Exception e)
            {
                throw new InvalidScoreException("Couldn't load score", e);
            }
        }
    }
}
