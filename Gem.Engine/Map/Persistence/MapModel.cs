using System;

namespace Gem.Map
{
    public class MapModel
    {
        public Guid Id
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int SizeX
        {
            get;
            set;
        }

        public int SizeY
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"";
        }
    }
}
