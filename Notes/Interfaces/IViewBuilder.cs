using System.Collections.Generic;
using Common.Definitions;
using Common.Models;

namespace Common.Interfaces
{
    public interface IViewBuilder<T>
    {
        void AddSymbolGroup(SymbolGroup group);
        void AddClef(Clefs clef);
        void Reset();
        T Build();
    }
}
