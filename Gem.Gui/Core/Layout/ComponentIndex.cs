namespace Gem.Gui.Core.Layout
{
    public class ComponentIndex
    {
        private readonly int lowest;
        private readonly int largest;
        private int current = 0;

        public ComponentIndex(int largest, int lowest = 0)
        {
            this.largest = largest;
            this.lowest = lowest;
        }

        public int Current
        {
            get
            {
                return current;
            }
            set
            {
                if (value > largest)
                {
                    current = lowest;
                }
                else if (value < lowest)
                {
                    current = largest;
                }
                else
                {
                    current = value;
                }
            }
        }

        public int Next
        {
            get { return Current++; }
        }

        public int Previous
        {
            get { return Current--; }
        }
    }
}
