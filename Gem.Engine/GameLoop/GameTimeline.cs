using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.GameLoop
{
    class GameTimeline : ITimeline
    {
        public double DeltaTime
        {
            get; set;
        }
    }
}
