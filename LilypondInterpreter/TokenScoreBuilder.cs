using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using Common.Utils;
using Repeat = LilypondInterpreter.Tokens.Repeat;

namespace LilypondInterpreter
{
    public class TokenScoreBuilder : IBuilder<Score>
    {
        private readonly List<string> _notes = new List<string> { "c", "d", "e", "f", "g", "a", "b" };
        private readonly List<string> _crosses = new List<string> { "cis", "dis", "fis", "gis", "ais" };
        private readonly List<string> _flats = new List<string> { "des", "es", "ges", "as", "bes" };

        private readonly Score _score;

        private SymbolGroup _currentGroup;

        private Common.Definitions.Octaves _relativeOctave;
        private TimeSignature _lastTimeSignature;
        private int _lastTempo;

        private Common.Models.Note _previous;

        public TokenScoreBuilder()
        {
            _currentGroup = new SymbolGroup();
            _score = new Score { SymbolGroups = { _currentGroup } };
            _relativeOctave = Octaves.Three;
            _previous = new Common.Models.Note(Names.C, _relativeOctave);
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

        /**
         * Format: 4=120
         */
        private void SetTempo(string value)
        {
            if (!int.TryParse(value.TrimStart('4', '='), out _lastTempo)) return;

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
            { Ticks = ticks, Beat = DurationUtils.GetClosestDuration(beat) };

            if (_currentGroup.Symbols.Count == 0) // still empty
            {
                _currentGroup.Meter = _lastTimeSignature;
                return;
            }

            CreateNewSymbolGroup();
        }

        private void AlterOctave(string value)
        {
            var name = Regex.Replace(value, @"[\d',.]", string.Empty)[0];
            switch (_previous.Name)
            {
                case Names.C when name == 'g' || name == 'a' || name == 'b':
                case Names.D when name == 'a' || name == 'b':
                case Names.E when name == 'b':
                    _relativeOctave--;
                    break;
                case Names.G when name == 'c':
                case Names.A when name == 'c' || name == 'd':
                case Names.B when name == 'c' || name == 'd' || name == 'e':
                    _relativeOctave++;
                    break;
            }

            var higher = value.ToCharArray().Count(a => a == '\'');
            if (higher > 0)
            {
                _relativeOctave += higher;
            }

            var lower = value.ToCharArray().Count(c => c == ',');
            if (lower > 0)
            {
                _relativeOctave -= lower;
            }
        }

        private void SetRelative(string value)
        {
            AlterOctave(value);
            _previous = new Common.Models.Note(Names.C, _relativeOctave);
        }

        private void SetClef(string value)
        {
            switch (value)
            {
                case "alto":
                    _score.Clef = Common.Definitions.Clefs.Alto;
                    break;
                case "bass":
                    _score.Clef = Common.Definitions.Clefs.Bass;
                    break;
                case "treble":
                default:
                    _score.Clef = Common.Definitions.Clefs.Treble;
                    break;
            }
        }

        private Common.Models.Note GetNote(Note note)
        {
            var name = Regex.Replace(note.Value, @"[\d',.]", string.Empty);
            int.TryParse(Regex.Replace(note.Value, @"[A-Za-z',.]", string.Empty), out var duration);

            AlterOctave(note.Value);

            // set previous to the new note
            _previous = new Common.Models.Note((Names)name[0], _relativeOctave, DurationUtils.GetClosestDuration(duration));

            if (name.EndsWith("es") || name.EndsWith("as"))
            {
                _previous.Modifier = Common.Definitions.Modifiers.Flat;
            }
            else if (name.EndsWith("is"))
            {
                _previous.Modifier = Common.Definitions.Modifiers.Sharp;
            }

            _previous.Dots = note.Value.ToCharArray().Count(dot => dot == '.');
            return _previous;
        }

        public void AddNote(Note note)
        {
            _currentGroup.Symbols.Add(GetNote(note));
        }

        private Common.Models.Rest GetRest(Rest rest)
        {
            if (int.TryParse(rest.Value.TrimStart('r'), out var duration))
            {
                return new Common.Models.Rest(DurationUtils.GetClosestDuration(duration));
            }
            return null;
        }

        public void AddRest(Rest rest)
        {
            var symbol = GetRest(rest);
            if (symbol != null)
            {
                _currentGroup.Symbols.Add(symbol);
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

        public void AddRepeat(Repeat repeat)
        {
            _currentGroup = new SymbolGroup
            {
                Meter = _score.SymbolGroups.Last().Meter,
                Tempo = _score.SymbolGroups.Last().Tempo,
                Repeat = new Common.Models.Repeat
                {
                    Times = repeat.Times
                }
            };

            var visitor = new TokenVisitor(this);
            foreach (var token in repeat.Inner)
            {
                token.Accept(visitor);
            }

            foreach (var alt in repeat.Alternatives)
            {
                _currentGroup.Repeat.Alternatives.Add(new SymbolGroup
                {
                    Meter = _score.SymbolGroups.Last().Meter,
                    Tempo = _score.SymbolGroups.Last().Tempo,
                });

                foreach (var token in alt)
                {
                    if (token is Note note)
                    {
                        _currentGroup.Repeat.Alternatives.Last().Symbols.Add(GetNote(note));
                    }

                    if (token is Rest rest)
                    {
                        _currentGroup.Repeat.Alternatives.Last().Symbols.Add(GetRest(rest));
                    }
                }
            }

            _score.SymbolGroups.Add(_currentGroup);
        }
    }
}