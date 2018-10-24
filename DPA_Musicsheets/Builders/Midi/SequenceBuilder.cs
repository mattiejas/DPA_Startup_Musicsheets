using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreModel = Common.Models.Score;

namespace DPA_Musicsheets.Builders.Midi
{
    public class SequenceBuilder : IBuilder<Sequence>
    {
        private ScoreModel _score;

        public SequenceBuilder(ScoreModel score)
        {
            _score = score;
        }

        public Sequence Build()
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            foreach (var symbolGroup in _score.SymbolGroups)
            {
                int _bpm = symbolGroup.Tempo == 0 ? 120 : symbolGroup.Tempo;            // Aantal beatnotes per minute.
                int _beatNote = (int)(symbolGroup.Meter?.Beat ?? Durations.Quarter);    // De waarde van een beatnote.
                int _beatsPerBar = symbolGroup.Meter?.Ticks ?? 4;                       // Aantal beatnotes per maat.

                int speed = (60000000 / _bpm);
                byte[] tempo = new byte[3];
                tempo[0] = (byte)((speed >> 16) & 0xff);
                tempo[1] = (byte)((speed >> 8) & 0xff);
                tempo[2] = (byte)(speed & 0xff);

                metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

                byte[] timeSignature = new byte[4];
                timeSignature[0] = (byte)_beatsPerBar;
                timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
                metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));

                Track notesTrack = new Track();
                sequence.Add(notesTrack);

                foreach (var symbol in symbolGroup.Symbols)
                {
                    // Calculate duration
                    double absoluteLength = 1.0 / (double)symbol.Duration;
                    absoluteLength += (absoluteLength / 2.0) * symbol.Dots;

                    double relationToQuartNote = _beatNote / 4.0;
                    double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                    double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                    if (symbol is Rest _)
                    {
                        absoluteTicks += (int)deltaTicks;
                    }

                    // Calculate height
                    if (symbol is Note note)
                    {
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Name.ToString().ToLower()) + ((int)note.Octave + 1) * 12;

                        int modifier = 0;
                        if (note.Modifier == Modifiers.Flat) modifier = -1;
                        else if (note.Modifier == Modifiers.Sharp) modifier = 1;

                        noteHeight += modifier;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume
                    }
                }
                notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
                metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            }

            return sequence;
        }
    }
}
