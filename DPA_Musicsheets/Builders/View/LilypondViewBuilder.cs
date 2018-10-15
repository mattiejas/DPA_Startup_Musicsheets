using Common.Definitions;
using Common.Interfaces;
using Common.Models;
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
        private Octaves _lastOctave;
        private TimeSignature _lastTimeSignature;

        private const int SPACES_IN_TAB = 2;

        public LilypondViewBuilder()
        {
            Init();
        }

        private void Init()
        {
            _output = "\\relative c' {\n";
            _scopes = 1;
            _lastIsKeyword = true;
        }

        public void AddClef(Clefs clef)
        {
            if (!_lastIsKeyword)
            {
                _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}{{\n";
                _scopes++;
            }
            _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}\\clef {clef.ToString().ToLower()}\n";
            _lastIsKeyword = true;
        }

        public void AddNote(Note note)
        {
            _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}{(char)note.Name}{(int)note.Duration}{new string('.', note.Dots)} ";
            _lastIsKeyword = false;
        }

        public void AddRest(Rest rest)
        {
            _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}r{(int)rest.Duration}{new string('.', rest.Dots)} ";
            _lastIsKeyword = false;
        }

        public void AddTimeSignature(TimeSignature timeSignature)
        {
            if (timeSignature == _lastTimeSignature) return;
            if (!_lastIsKeyword)
            {
                _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}{{\n";
                _scopes++;
            }
            _output += $"{new string(' ', _scopes * SPACES_IN_TAB)}\\time {timeSignature.Ticks}/{(int)timeSignature.Beat}\n";
            _lastTimeSignature = timeSignature;
            _lastIsKeyword = true;
        }

        private void CloseScopes()
        {
            while(_scopes > 0)
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
