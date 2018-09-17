using System;
using System.Collections.Generic;
using System.Text;
using Notes.Definitions;

namespace Notes.Models
{
    public class Score
    {
        public List<SymbolGroup> SymbolGroups { get; set; }
        public Clefs Clef { get; set; }

        public Score(Clefs clef = Clefs.Treble)
        {
            SymbolGroups = new List<SymbolGroup>();
            this.Clef = clef;
        }
    }
}
