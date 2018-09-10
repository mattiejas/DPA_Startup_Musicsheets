using Notes.Definitions;

namespace Notes.Models
{
    public class Note : Symbol
    {
        private Name _name;
        private Octave _octave;

        public Symbol SetNote(Name name, Octave octave)
        {
            _name = name;
            _octave = octave;
            return this;
        }
    }
}
