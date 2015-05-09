using System;

namespace Gem.Gui.Factories
{
    public class GeneralControlFactory : IControlFactory
    {
        public Controls.ImageButton CreateImageButton()
        {
            throw new NotImplementedException();
        }
        
        public Controls.Label CreateLabel()
        {
            throw new NotImplementedException();
        }

        public Controls.TextField CreateTextBox()
        {
            throw new NotImplementedException();
        }

        public Controls.Button CreateButton(Rendering.Region region, Func<Rendering.Region, Microsoft.Xna.Framework.Vector2> originCalculator, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            throw new NotImplementedException();
        }
    }
}
