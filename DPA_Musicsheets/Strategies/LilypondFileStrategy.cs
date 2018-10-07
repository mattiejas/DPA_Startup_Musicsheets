using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using DPA_Musicsheets.Builders.Score;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Strategies
{
    class LilypondFileStrategy : IFileStrategy
    {
        private readonly IViewManagerPool _pool;
        private IScoreBuilder _builder;

        public LilypondFileStrategy(IViewManagerPool pool)
        {
            _pool = pool;
        }

        public void Handle(string filename)
        {
            _builder = new LilypondScoreBuilder(filename);
            var score = _builder.Build();

            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }
        }
    }
}
