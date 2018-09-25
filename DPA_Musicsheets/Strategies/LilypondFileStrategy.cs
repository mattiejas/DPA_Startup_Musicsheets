using System.IO;
using System.Text;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Strategies
{
    class LilypondFileStrategy : IFileStrategy
    {
        public string LilypondText { get; set; }
        public LilypondViewModel LilypondViewModel { get; set; }

        public void Handle(string filename)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(filename))
            {
                sb.AppendLine(line);
            }

            this.LilypondText = sb.ToString();
            this.LilypondViewModel.LilypondTextLoaded(this.LilypondText);
        }
    }
}
