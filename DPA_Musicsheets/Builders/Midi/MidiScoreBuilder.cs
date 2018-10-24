using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using Common.Utils;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Builders.Midi
{
    public class MidiScoreBuilder : IScoreBuilder
    {
        private readonly Sequence _sequence;
        private TimeSignature _currentTimeSignature;
        private int _previousTicks;
        private bool _startedNoteIsClosed;

        private List<Symbol> _symbols;

        internal class MetaSymbolGroup
        {
            public int Start { get; set; }
            public int? End { get; set; }
            public SymbolGroup SymbolGroup { get; set; }
        }

        internal struct Duration
        {
            public Durations Length;
            public int Dots;
        }

        public MidiScoreBuilder(Sequence sequence)
        {
            _sequence = sequence;
            _startedNoteIsClosed = true;
            _previousTicks = 0;
            _symbols = new List<Symbol>();
        }

        public Common.Models.Score Build()
        {
            var symbolGroups = GetMetadataFromTrack(_sequence[0]);

            var score = new Common.Models.Score()
            {
                Clef = Clefs.Treble
            };

            foreach (var meta in symbolGroups)
            {
                _previousTicks = meta.Start;
                meta.SymbolGroup.Symbols = GetSymbolsFromTrack(_sequence[1], _sequence.Division, meta.SymbolGroup.Meter, meta.Start, meta.End);
                score.SymbolGroups.Add(meta.SymbolGroup);
            }

            return score;
        }

        private List<Symbol> GetSymbolsFromTrack(Track track, int division, TimeSignature timeSignature, int start, int? end)
        {
            _symbols = new List<Symbol>();
            _startedNoteIsClosed = true;

            foreach (var midiEvent in track.Iterator())
            {
                IMidiMessage midiMessage = midiEvent.MidiMessage;
                if (midiEvent.AbsoluteTicks < start) continue;

                if (end != null && midiEvent.AbsoluteTicks >= end) return _symbols;

                if (midiMessage.MessageType != MessageType.Channel) continue;

                if (midiEvent.MidiMessage is ChannelMessage channelMessage)
                {
                    if (channelMessage.Command == ChannelCommand.NoteOn)
                    {
                        StartNote(channelMessage, midiEvent.AbsoluteTicks, division);
                    }
                    else if (channelMessage.Command == ChannelCommand.NoteOff)
                    {
                        CloseNote(_symbols[_symbols.Count - 1], midiEvent.AbsoluteTicks, division);
                    }
                }
            }

            return _symbols;
        }

        private void StartNote(ChannelMessage message, int absoluteTicks, int division)
        {
            if (message.Data2 > 0) // Data2 = loudness
            {
                // check if there is time between the last note event and this one
                var delta = absoluteTicks - _previousTicks;
                if (delta > 0)
                {
                    var rest = new Rest();
                    var duration = GetDuration(_previousTicks, absoluteTicks, division);
                    if (duration.HasValue)
                    {
                        rest.Duration = duration.Value.Length;
                        rest.Dots = duration.Value.Dots;
                    }
                    _symbols.Add(rest);
                    _previousTicks = absoluteTicks;
                }

                // Append the new note.
                _symbols.Add(GetNoteFromMidiKey(message.Data1));
                _startedNoteIsClosed = false;
                return;
            }

            if (!_startedNoteIsClosed)
            {
                CloseNote(_symbols[_symbols.Count - 1], absoluteTicks, division);
                _previousTicks = absoluteTicks;
                _startedNoteIsClosed = true;
                return;
            }

            _symbols.Add(new Rest { Duration = Durations.Quarter });
        }

        private void CloseNote(Symbol symbol, int absoluteTicks, int division)
        {
            var duration = GetDuration(_previousTicks, absoluteTicks, division);

            if (duration.HasValue)
            {
                symbol.Duration = duration.Value.Length;
                symbol.Dots = duration.Value.Dots;
            }

            _previousTicks = absoluteTicks;
            _startedNoteIsClosed = true;
        }

        private IList<MetaSymbolGroup> GetMetadataFromTrack(Track track)
        {
            var symbolGroups = new List<MetaSymbolGroup>();
            MetaSymbolGroup last = null;

            foreach (var e in track.Iterator())
            {
                var message = e.MidiMessage;
                if (message.MessageType != MessageType.Meta) continue;
                var metaMessage = message as MetaMessage;

                switch (metaMessage?.MetaType)
                {
                    case MetaType.TimeSignature:
                        if (last != null) last.End = e.AbsoluteTicks;
                        var timeSignatureBytes = metaMessage.GetBytes();
                        var beat = 1 / Math.Pow(timeSignatureBytes[1], -2);

                        var symbolGroup = new SymbolGroup
                        {
                            Meter = new TimeSignature
                            {
                                Ticks = timeSignatureBytes[0],
                                Beat = DurationUtils.GetClosestDuration(beat)
                            }
                        };

                        var meta = new MetaSymbolGroup { Start = e.AbsoluteTicks, SymbolGroup = symbolGroup };
                        symbolGroups.Add(meta);
                        last = meta;
                        _currentTimeSignature = symbolGroup.Meter;
                        break;
                    case MetaType.Tempo:
                        var tempoBytes = metaMessage.GetBytes();
                        var tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 |
                                    (tempoBytes[2] & 0xff);
                        last.SymbolGroup.Tempo = 60000000 / tempo; // bpm
                        break;
                }
            }

            return symbolGroups;
        }

        private Note GetNoteFromMidiKey(int midiKey)
        {
            Names name;
            var octave = (Octaves)(midiKey / 12 - 1);
            Modifiers? modifier = null;

            switch (midiKey % 12)
            {
                case 0:
                    name = Names.C;
                    break;
                case 1:
                    name = Names.C;
                    modifier = Modifiers.Sharp;
                    break;
                case 2:
                    name = Names.D;
                    break;
                case 3:
                    name = Names.D;
                    modifier = Modifiers.Sharp;
                    break;
                case 4:
                    name = Names.E;
                    break;
                case 5:
                    name = Names.F;
                    break;
                case 6:
                    name = Names.F;
                    modifier = Modifiers.Sharp;
                    break;
                case 7:
                    name = Names.G;
                    break;
                case 8:
                    name = Names.G;
                    modifier = Modifiers.Sharp;
                    break;
                case 9:
                    name = Names.A;
                    break;
                case 10:
                    name = Names.A;
                    modifier = Modifiers.Sharp;
                    break;
                case 11:
                    name = Names.B;
                    break;
                default:
                    name = Names.C;
                    break;
            }

            var note = new Note(name, octave)
            {
                Modifier = modifier
            };

            return note;
        }

        private Duration? GetDuration(int absoluteTicks, int nextNoteAbsoluteTicks, int division)
        {
            int duration = 0;
            int dots = 0;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return null;
            }

            double percentageOfBeatNote = deltaTicks / division;
            var percentageOfBar = (1.0 / (int)_currentTimeSignature.Beat) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2)
                        noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32)
                        subtractDuration = 32;
                    else if (noteLength >= 16)
                        subtractDuration = 16;
                    else if (noteLength >= 8)
                        subtractDuration = 8;
                    else if (noteLength >= 4)
                        subtractDuration = 4;
                    else
                        subtractDuration = 2;

                    if (noteLength >= 17)
                        duration = 32;
                    else if (noteLength >= 9)
                        duration = 16;
                    else if (noteLength >= 5)
                        duration = 8;
                    else if (noteLength >= 3)
                        duration = 4;
                    else
                        duration = 2;

                    double currentTime = 0;

                    while (currentTime < (noteLength - subtractDuration))
                    {
                        var addtime = 1 / ((subtractDuration / _currentTimeSignature.Ticks) * Math.Pow(2, dots));
                        if (addtime <= 0) break;
                        currentTime += addtime;
                        if (currentTime <= (noteLength - subtractDuration))
                        {
                            dots++;
                        }
                        if (dots >= 4) break;
                    }

                    break;
                }
            }

            return new Duration { Length = (Durations)duration, Dots = dots };
        }
    }
}