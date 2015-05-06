namespace Gem.Gui.Controls
{
    public class AggregationToken
    {
        public bool HasHover { get; set; }

        public bool HasFocus { get; set; }

        public bool IsSelected { get; set; }

        public object GotFocusBy { get; set; }

        public bool HasGottenFocusBy<TModifier>(TModifier instance)
            where TModifier : class
        {
            return (GotFocusBy as TModifier) != null;
        }
    }
}
