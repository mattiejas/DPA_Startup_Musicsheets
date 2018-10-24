using System.Collections.Generic;
using System.IO;
using Common.Interfaces;
using DPA_Musicsheets.Strategies;

namespace DPA_Musicsheets.Managers
{
    public class MusicLoader
    {
        private static Dictionary<string, IFileStrategy> _strategies;

        public MusicLoader(IViewManagerPool pool)
        {
            _strategies = new Dictionary<string, IFileStrategy>
            {
                {".mid", new MidiFileStrategy(new MidiLoadStrategy(pool))},
                { ".ly", new LilypondFileStrategy(new LilypondLoadStrategy(pool))}
            };
        }

        public void Load(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension != null && _strategies.ContainsKey(extension))
            {
                _strategies[extension].Handle(fileName);
            }
        }
    }
}
