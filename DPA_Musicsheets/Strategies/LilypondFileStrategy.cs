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
        private readonly IScoreBuilder<string> _builder;

        public string LilypondText { get; set; }
        public LilypondViewModel LilypondViewModel { get; set; }

        public LilypondFileStrategy(IViewManagerPool pool)
        {
            _pool = pool;
            _builder = new LilypondScoreBuilder();
        }

        public void Handle(string filename)
        {
            var score = _builder.Build(filename);

            foreach (var viewManager in _pool)
            {
                viewManager.Load(score);
            }

            /*
                Verplaatst naar LilypondViewManager
                StringBuilder sb = new StringBuilder();
                foreach (var line in File.ReadAllLines(filename))
                {
                    sb.AppendLine(line);
                }

                this.LilypondText = sb.ToString();
                this.LilypondViewModel.LilypondTextLoaded(this.LilypondText);
            */
        }
    }
}
