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
    class LilypondScoreBuilder : IScoreBuilder
    {
        private readonly string _input;

        public LilypondScoreBuilder(string input)
        {
            _input = input;
        }

        public Common.Models.Score Build()
        {
            var tokens = LilypondInterpreter.Tokenizer.Tokenize(_input);
            var score = LilypondInterpreter.Interpreter.Interpret(tokens);
            
            return score;
        }
    }
}
 