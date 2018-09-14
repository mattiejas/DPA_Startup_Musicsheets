using System;
using System.Collections.Generic;
using System.Text;
using Notes.Definitions;

namespace Notes.Models
{
    public class TimeSignature
    {
        public int Ticks { get; set; }
        public Durations Beat { get; set; }
    }
}
