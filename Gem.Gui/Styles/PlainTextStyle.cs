using Gem.Gui.Controls;
using Gem.Gui.Text;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Core.Styles
{
    public class PlainTextStyle : IRenderStyle
    {

        #region Fields

        private readonly IText text;
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();

        #endregion

        public PlainTextStyle(IText text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.text = text;
        }

        private void Flush()
        {
            foreach (var transformation in activeTransformations)
            {
                transformation.Dispose();
            }
        }

        #region Style

        public void Focus()
        {
            return;
        }

        public void Default()
        {
            return;
        }

        public void Hover()
        {
            return;
        }
        
        public void Clicked()
        {
            return;
        }

        #endregion

    }
}
