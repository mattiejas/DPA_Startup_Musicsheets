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

        private const int SPACES_IN_TAB = 2;

        public LilypondViewBuilder()
        {
            Init();
        }

        private void Init()
        {
            _scopes = 1;
            _output = $"\\relative c' {{\n{new string(' ', _scopes * SPACES_IN_TAB)}";
            _lastIsKeyword = true;
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

        public void AddNote(Note note)
        {
            _output += $"{(char)note.Name}{(int)note.Duration}{new string('.', note.Dots)} ";
            _lastIsKeyword = false;

            SetBarlineProgress((double)note.Duration, note.Dots);
        }

        public void AddRest(Rest rest)
        {
            _output += $"r{(int)rest.Duration}{new string('.', rest.Dots)} ";
            _lastIsKeyword = false;

            SetBarlineProgress((double)rest.Duration, rest.Dots);
        }

        public void SetBarlineProgress(double duration, int dots)
        {
            _progress -= DurationUtils.GetProgressDuration((double)_lastTimeSignature.Beat / duration, dots);

            if (_progress <= 0)
            {
                _output += $" |\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _progress = _lastTimeSignature.Ticks;
            }
        }

        public void AddTimeSignature(TimeSignature timeSignature)
        {
            if (timeSignature == _lastTimeSignature) return;
            if (!_lastIsKeyword)
            {
                _output += $"{{\n{new string(' ', _scopes * SPACES_IN_TAB)}";
                _scopes++;
            }
            _output += $"\\time {timeSignature.Ticks}/{(int)timeSignature.Beat}\n{new string(' ', _scopes * SPACES_IN_TAB)}";
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
            return _output;
        }

        public void Reset()
        {
            Init();
        }
    }
}
