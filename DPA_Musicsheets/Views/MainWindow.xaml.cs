using PSAMControlLibrary;
using System.Windows;
using Notes.Definitions;
using Notes.Models;
using Duration = Notes.Definitions.Duration;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // TODO: testing domain
            var note = new Notes.Models.Note()
                .SetNote(Notes.Definitions.Name.C, Octave.Four)
                .SetDuration(Duration.Whole)
                .AddModifier(Modifier.Flat)
                .AddModifier(Modifier.Dotted)
                .AddModifier(Modifier.Dotted);

            var rest = new Notes.Models.Rest()
                .SetDuration(Duration.Half)
                .AddModifier(Modifier.Dotted);
        }
    }
}
