using Common.Definitions;
using Common.Interfaces;
using Common.Models;
using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders.View
{
    public class LilypondViewBuilder : IViewBuilder<string>
    {
        private string _output;
        private int _scopes;
        private bool _lastIsKeyword;
        private TimeSignature _lastTimeSignature;
        private double _progress;
        private int _lastTempo;
        private Note _previousNote;

        private string _relative;

        private const int SPACES_IN_TAB = 2;

        public LilypondViewBuilder()
        {
            Init();
        }

        private void Init()
        {
            _scopes = 1;
            _output = new string(' ', _scopes * SPACES_IN_TAB);
            _lastIsKeyword = true;
            _lastTempo = 0;
            _previousNote = new Note(Names.C, Octaves.Three);
            _relative = null;
        }

        public void AddClef(Clefs clef)
        {
            if (!_lastIsKeyword)
            {
                _output += $"{{\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _scopes++;
            }

            _output += $"\\clef {clef.ToString().ToLower()}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            _lastIsKeyword = true;
        }

        private void AddTempo(int tempo)
        {
            if (_lastTempo != tempo)
            {
                _output += $"\\tempo 4={tempo}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _lastTempo = tempo;
            }
        }

        private string GetModifier(Note note)
        {
            if (note.Modifier == null) return "";

            if (note.Modifier == Modifiers.Flat)
            {
                if (note.Name == Names.A || note.Name == Names.E)
                {
                    return "s";
                }

                return "as";
            }

            if (note.Modifier == Modifiers.Sharp)
            {
                return "is";
            }

            return "";
        }

        private Octaves GetOctave(Note note)
        {
            switch (_previousNote.Name)
            {
                case Names.C when note.Name == Names.G || note.Name == Names.A || note.Name == Names.B:
                case Names.D when note.Name == Names.A || note.Name == Names.B:
                case Names.E when note.Name == Names.B:
                    return _previousNote.Octave - 1;
                case Names.G when note.Name == Names.C:
                case Names.A when note.Name == Names.C || note.Name == Names.D:
                case Names.B when note.Name == Names.C || note.Name == Names.D || note.Name == Names.E:
                    return _previousNote.Octave + 1;
            }

            return _previousNote.Octave;
        }

        private void AddNote(Note note)
        {
            if (_relative == null)
            {
                var diff = note.Octave - GetOctave(note);
                _relative = $"\\relative c{(diff > 0 ? new string('\'', diff) : new string(',', -diff))} {{\n";
                _previousNote = new Note(Names.C, Octaves.Three + diff);
            }

            var difference = note.Octave - GetOctave(note);
            _output += $"{(char) note.Name}" +
                       $"{GetModifier(note)}" +
                       $"{(difference > 0 ? new string('\'', difference) : new string(',', -difference))}" +
                       $"{(int) note.Duration}" +
                       $"{new string('.', note.Dots)} ";
            _lastIsKeyword = false;
            _previousNote = note;

            SetBarlineProgress((double) note.Duration, note.Dots);
        }

        private void AddRest(Rest rest)
        {
            _output += $"r{(int) rest.Duration}{new string('.', rest.Dots)} ";
            _lastIsKeyword = false;

            SetBarlineProgress((double) rest.Duration, rest.Dots);
        }

        private void SetBarlineProgress(double duration, int dots)
        {
            _progress -= DurationUtils.GetProgressDuration((double) _lastTimeSignature.Beat / duration, dots);

            if (_progress <= 0)
            {
                _output += $" |\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _progress = _lastTimeSignature.Ticks;
            }
        }

        private void AddTimeSignature(TimeSignature timeSignature)
        {
            if (timeSignature == _lastTimeSignature) return;
            if (!_lastIsKeyword)
            {
                _output += $"{{\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _scopes++;
            }

            _output +=
                $"\\time {timeSignature.Ticks}/{(int) timeSignature.Beat}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            _lastTimeSignature = timeSignature;
            _lastIsKeyword = true;
            _progress = timeSignature.Ticks;
        }

        private void CloseScopes()
        {
            _output = _output.TrimEnd(' ');
            _output = _output.TrimEnd('\n');
            while (_scopes > 0)
            {
                _output += $"\n{new string(' ', --_scopes * SPACES_IN_TAB)}}}";
            }
        }

        public string Build()
        {
            CloseScopes();
            return _relative + _output;
        }

        public void Reset()
        {
            Init();
        }

        public void AddSymbolGroup(SymbolGroup group)
        {
            AddTimeSignature(group.Meter);
            AddTempo(group.Tempo);

            if (group.Repeat != null)
            {
                _scopes++;
                _output = _output.TrimEnd(' ', '\n', '|');
                _output += $"\n{new string(' ', _scopes * SPACES_IN_TAB)}\\repeat volta {group.Repeat.Times} {{\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            }

            foreach (var symbol in group.Symbols)
            {
                if (symbol is Note note)
                {
                    AddNote(note);
                }

                if (symbol is Rest rest)
                {
                    AddRest(rest);
                }
            }

            if (group.Repeat != null)
            {
                _output = _output.TrimEnd(' ', '\n', '|');
                _output += $"\r{new string(' ', --_scopes * SPACES_IN_TAB)}}}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            }

            if (group.Repeat != null)
            {
                AddRepeat(group.Repeat);
            }
        }

        private void AddRepeat(Repeat repeat)
        {
            _scopes++;
            _output += $"\\alternative {{\n{new string(' ', _scopes * SPACES_IN_TAB)}";

            foreach (var alt in repeat.Alternatives)
            {
                _scopes++;
                _output += "{ ";
                AddSymbolGroup(alt);

                _scopes--;
                _output = _output.TrimEnd(' ', '\n', '|');
                _output += $" }}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            }

            _scopes--;
            _output = _output.TrimEnd(' ', '\n', '|');
            _output += $"\r}}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
        }
    }
}