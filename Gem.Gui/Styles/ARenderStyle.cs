using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Styles
{
    public abstract class ARenderStyle
    {

        #region Fields

        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();
        private bool showedFocusAnimation;

        #endregion


        private void Flush()
        {
            foreach (var transformation in activeTransformations)
            {
                transformation.Dispose();
            }
            showedFocusAnimation = false;
        }

        #region Styles

        protected virtual Func<IRenderable, ITransformation> FocusStyle
        {
            get { return control => new NoTransformation(); }
        }
        protected virtual Func<IRenderable, ITransformation> DefaultStyle
        {
            get { return control => new NoTransformation(); }
        }

        protected virtual Func<IRenderable, ITransformation> HoverStyle
        {
            get { return control => new NoTransformation(); }
        }

        protected virtual Func<IRenderable, ITransformation> ClickedStyle
        {
            get { return control => new NoTransformation(); }
        }

        #endregion

        public void Focus(AControl styleControl)
        {
            if (showedFocusAnimation) return;

            Flush();
            activeTransformations.Add(styleControl.AddTransformation(FocusStyle(styleControl)));
            showedFocusAnimation = true;
        }
        public void Default(AControl styleControl)
        {
            if (styleControl.HasFocus) return;
            Flush();
            activeTransformations.Add(styleControl.AddTransformation(DefaultStyle(styleControl)));

        }
        public void Hover(AControl styleControl)
        {
            if (styleControl.HasFocus) return;
            Flush();
            activeTransformations.Add(styleControl.AddTransformation(HoverStyle(styleControl)));
        }
        public void Clicked(AControl styleControl)
        {
            Flush();
            activeTransformations.Add(styleControl.AddTransformation(ClickedStyle(styleControl)));
        }

        public abstract void Render(IRenderable renderable, SpriteBatch batch);
    }
}
