using System.Collections.Generic;
using Notes.Definitions;

namespace Notes.Models
{
    public class Note : Symbol
    {
        public Modifiers Modifiers { get; set; }
        public Names Name { get; set; }
        public Octaves Octave { get; set; }
    }
}
