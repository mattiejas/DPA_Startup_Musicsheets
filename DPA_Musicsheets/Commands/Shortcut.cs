using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands
{
    public class Shortcut
    {
        private readonly HashSet<Key> _pressed;

        public Shortcut(params Key[] keys)
        {
            _pressed = new HashSet<Key>();

            foreach (var key in keys)
            {
                _pressed.Add(key);
            }
        }

        public HashSet<Key> GetPressed()
        {
            return _pressed;
        }

        public void Clear()
        {
            _pressed.Clear();
        }

        public void Add(Key key)
        {
            _pressed.Add(key);
        }

        public void Remove(Key key)
        {
            _pressed.Remove(key);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Shortcut shortcut)) return false;

            var pressed = shortcut.GetPressed();
            if (_pressed.Count != pressed.Count) return false;

            foreach (var key in _pressed)
            {
                if (!pressed.Contains(key)) return false;
            }

            return true;
        }

        public bool Contains(Shortcut shortcut)
        {
            foreach (var key in shortcut.GetPressed())
            {
                if (!_pressed.Contains(key)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _pressed.GetHashCode();
        }
    }
}