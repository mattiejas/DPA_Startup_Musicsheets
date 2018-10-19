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
        private readonly ILoadStrategy<string> _loader;

        public LilypondFileStrategy(ILoadStrategy<string> loader)
        {
            _loader = loader;
        }

        public void Handle(string filename)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(filename))
            {
                sb.AppendLine(line);
            }

            _loader.Load(sb.ToString());
        }
    }
}
