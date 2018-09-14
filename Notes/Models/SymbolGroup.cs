using System;
using System.Collections.Generic;
using System.Text;
using Notes.Definitions;

namespace Notes.Models
{
    public class SymbolGroup
    {
        public List<Symbol> Symbols { get; set; }
        public Clefs Clef { get; set; }
        public int Tempo { get; set; }
        public TimeSignature Meter { get; set; }

        public Repeat Repeat { get; set; }

        public SymbolGroup(Clefs clef = Clefs.C)
        {
            Symbols = new List<Symbol>();
            Clef = clef;
        }
    }
}
