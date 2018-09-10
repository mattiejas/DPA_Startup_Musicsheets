using System;
using System.Collections.Generic;
using System.Linq;
using Notes.Definitions;

namespace Notes.Models
{
    public abstract class Symbol
    {
        private readonly List<Modifier> _modifiers = new List<Modifier>();
        private Duration _duration;

        public virtual Symbol AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            return this;
        }

        public Symbol SetDuration(Duration duration)
        {
            _duration = duration;
            return this;
        }
    }
}