using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Models
{
    public class Score
    {
        public List<SymbolGroup> SymbolGroups { get; set; }

        public Score()
        {
            SymbolGroups = new List<SymbolGroup>();
        }
    }
}
