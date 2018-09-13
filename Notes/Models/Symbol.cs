using System;
using System.Collections.Generic;
using System.Linq;
using Notes.Definitions;

namespace Notes.Models
{
    public abstract class Symbol
    {
        public Durations Duration { get; set; }
        public int Dots { get; set; }
    }
}