using Notes.Definitions;

namespace Notes.Models
{
    public class Rest : Symbol
    {
        public override Symbol AddModifier(Modifier modifier)
        {
            this.AddModifier();
            return this;
        }

        public Symbol AddModifier()
        {
            this.AddModifier(Modifier.Dotted);
            return this;
        }
    }
}