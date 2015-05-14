using Gem.Gui.Controls;

namespace Gem.Gui.Transformations
{
    public class NoTransformation : ITransformation
    {
        public bool Enabled { get { return false; } }

        public void Transform(AControl element, double deltaTime) { return; }
    }

}
