using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Notes.Definitions;
using Notes.Models;
using PSAMControlLibrary;
using Note = Notes.Models.Note;
using PSAMTimeSignature = PSAMControlLibrary.TimeSignature;
using PSAMNote = PSAMControlLibrary.Note;
using TimeSignature = Notes.Models.TimeSignature;

namespace DPA_Musicsheets.Managers
{
    public static class ViewManager
    {
        public static IList<MusicalSymbol> Load(Score score)
        {
            var builder = new PsamViewBuilder();
            builder.AddClef(score.Clef);
            builder.AddTimeSignature(score.SymbolGroups[0].Meter);

            foreach (var symbolGroup in score.SymbolGroups)
            {
                builder.AddTimeSignature(symbolGroup.Meter);

                foreach (var symbol in symbolGroup.Symbols)
                {
                    if (symbol is Note note)
                    {
                        builder.AddNote(note);
                    }
                }
            }

            return builder.Build();
        }
    }
}
