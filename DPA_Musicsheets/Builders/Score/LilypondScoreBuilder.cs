using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders.Score
{
    class LilypondScoreBuilder : IScoreBuilder<string>
    {
        public Common.Models.Score Build(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(input))
            {
                sb.AppendLine(line);
            }

            var context = LilypondInterpreter.Tokenizer.Tokenize(sb.ToString());

            return new Common.Models.Score();
        }
    }
}
