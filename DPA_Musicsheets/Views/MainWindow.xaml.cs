using System.Windows;
using Notes.Definitions;
using Notes.Models;

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
            var note = new Note()
            {
                Modifiers = Modifiers.Flat,
                Name = Names.A,
                Octave = Octaves.Eight,
                Dots = 0,
                Duration = Durations.Eight,
            };

            var rest = new Rest()
            {
                Duration = Durations.Quarter,
                Dots = 1
            };
        }
    }
}
