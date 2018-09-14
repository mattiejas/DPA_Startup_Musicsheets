using Notes.Definitions;

namespace Notes.Models
{
    public class Rest : Symbol
    {
        public Rest(Durations duration = Durations.Quarter) : base(duration)
        {
        }
    }
}