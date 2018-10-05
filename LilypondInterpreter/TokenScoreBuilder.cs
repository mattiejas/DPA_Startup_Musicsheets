﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using Common.Utils;

namespace LilypondInterpreter
{
    public class TokenScoreBuilder : IScoreBuilder
    {
        private readonly List<string> _notes = new List<string> {"c", "d", "e", "f", "g", "a", "b"};
        private readonly List<string> _crosses = new List<string> {"cis", "dis", "fis", "gis", "ais"};
        private readonly List<string> _flats = new List<string> {"des", "es", "ges", "as", "bes"};

        private readonly Score _score;

        private SymbolGroup _currentGroup;

        private Octaves _lastOctave;
        private TimeSignature _lastTimeSignature;
        private int _lastTempo;

        public TokenScoreBuilder()
        {
            _currentGroup = new SymbolGroup();
            _score = new Score {SymbolGroups = {_currentGroup}};
            _lastOctave = Octaves.Three;
        }

        public Score Build()
        {
            return _score;
        }

        private void CreateNewSymbolGroup()
        {
            // only when current group is not empty
            if (_currentGroup.Symbols.Count <= 0) return;

            _currentGroup = new SymbolGroup
            {
                Meter = _lastTimeSignature,
                Tempo = _lastTempo
            };
            _score.SymbolGroups.Add(_currentGroup);
        }

        public void AddKeyword(Keyword keyword)
        {
            switch (keyword.Type)
            {
                case Keywords.Relative:
                    SetRelative(keyword.Value);
                    break;
                case Keywords.Clef:
                    SetClef(keyword.Value);
                    break;
                case Keywords.TimeSignature:
                    SetTimeSignature(keyword.Value);
                    break;
                case Keywords.Tempo:
                    SetTempo(keyword.Value);
                    break;
                case Keywords.Repeat:
                    // TODO
                    break;
                case Keywords.Alternative:
                    // TODO
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetTempo(string value)
        {
            if (!int.TryParse(value, out _lastTempo)) return;

            if (_currentGroup.Symbols.Count == 0) // list is empty
            {
                _currentGroup.Tempo = _lastTempo;
                return;
            }

            CreateNewSymbolGroup();
        }

        private void SetTimeSignature(string value)
        {
            var values = value.Split('/');
            if (!int.TryParse(values[0], out var ticks) || !int.TryParse(values[1], out var beat)) return;

            _lastTimeSignature = new TimeSignature
                {Ticks = ticks, Beat = DurationUtils.GetClosestDuration(beat)};

            if (_currentGroup.Symbols.Count == 0) // still empty
            {
                _currentGroup.Meter = _lastTimeSignature;
                return;
            }

            CreateNewSymbolGroup();
        }

        private void AlterOctave(string value)
        {
            var higher = value.ToCharArray().Count(a => a == '\'');
            if (higher > 0)
            {
                _lastOctave += higher;
            }
            else
            {
                var lower = value.ToCharArray().Count(c => c == ',');
                _lastOctave -= lower;
            }
        }

        private void SetRelative(string value)
        {
            AlterOctave(value);
        }

        private void SetClef(string value)
        {
            switch (value)
            {
                case "alto":
                    _score.Clef = Clefs.Alto;
                    break;
                case "bass":
                    _score.Clef = Clefs.Bass;
                    break;
                case "treble":
                default:
                    _score.Clef = Clefs.Treble;
                    break;
            }
        }

        public void AddNote(Note note)
        {
            var name = Regex.Replace(note.Value, @"[\d',.]", string.Empty);
            int.TryParse(Regex.Replace(note.Value, @"[A-Za-z',.]", string.Empty), out var duration);

            AlterOctave(note.Value);

            var n = new Common.Models.Note(Names.C, _lastOctave, DurationUtils.GetClosestDuration(duration));

            if (name.EndsWith("es") || name.EndsWith("as"))
            {
                n.Modifier = Modifiers.Flat;
            }
            else if (name.EndsWith("is"))
            {
                n.Modifier = Modifiers.Sharp;
            }

            n.Dots = note.Value.ToCharArray().Count(dot => dot == '.');
            _currentGroup.Symbols.Add(n);
        }

        public void AddRest(Rest rest)
        {
            if (int.TryParse(rest.Value.TrimStart('r'), out var duration))
            {
                _currentGroup.Symbols.Add(new Common.Models.Rest(DurationUtils.GetClosestDuration(duration)));
            }
        }

        public void OpenNewScope()
        {
            CreateNewSymbolGroup();
        }

        public void CloseScope()
        {
            var count = _score.SymbolGroups.Count;
            if (count < 2) return;

            _currentGroup = new SymbolGroup
            {
                Meter = _score.SymbolGroups[count - 2].Meter,
                Tempo = _score.SymbolGroups[count - 2].Tempo
            };
            _score.SymbolGroups.Add(_currentGroup);
        }
    }
}