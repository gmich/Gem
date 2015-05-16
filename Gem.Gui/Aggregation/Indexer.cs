namespace Gem.Gui.Aggregation
{
    public class Indexer
    {
        private readonly int lowest;
        private readonly int largest;
        private int current;

        public Indexer(int lowest, int largest, int current = 0)
        {
            this.lowest = lowest;
            this.largest = largest;
            this.Current = current;
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

        public bool HasFocus { get; set; }

        public int Next
        {
            get { return ++Current; }
        }

        public int Previous
        {
            get { return --Current; }
        }
    }
}
