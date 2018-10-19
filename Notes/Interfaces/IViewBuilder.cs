using System.Collections.Generic;
using Common.Definitions;
using Common.Models;

namespace Common.Interfaces
{
    public interface IViewBuilder<T>
    {
        void AddNote(Note note);
        void AddRest(Rest rest);
        void AddClef(Clefs clef);
        void AddTempo(int tempo);
        void AddTimeSignature(TimeSignature timeSignature);

        void Reset();
        T Build();
    }
}
