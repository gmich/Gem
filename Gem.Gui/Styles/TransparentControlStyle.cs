using System;
using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Gem.Gui.Utilities;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Gem.Gui.Rendering;

namespace Gem.Gui.Styles
{
    public sealed class TransparentControlStyle : ARenderStyle
    {

        public TransparentControlStyle()
        {
            this.AssignDefaultValues();
        }

        public TransparentControlStyle(float focusAlpha, float hoverAlpha, float defaultAlpha)
        {
            this.FocusAlpha = focusAlpha;
            this.HoverAlpha = hoverAlpha;
            this.DefaultAlpha = defaultAlpha;
            this.AlphaLerpStep = 4.0f;
        }

        #region Properties

        [DefaultValue(1.0f)]
        public float FocusAlpha { get; set; }

        [DefaultValue(0.8f)]
        public float HoverAlpha { get; set; }

        [DefaultValue(0.6f)]
        public float DefaultAlpha { get; set; }

        [DefaultValue(4.0f)]
        public float AlphaLerpStep { get; set; }

        #endregion

        #region Style

        protected override Func<IRenderable, ITransformation> FocusStyle
        {
            get
            {
                return
                    ctrl => new PredicateTransformation(
                            expirationPredicate: control =>
                            control.RenderParameters.Transparency == FocusAlpha,
                            transformer: (timeDelta, control) =>
                            control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                                    .Approach(FocusAlpha
                                                                             , (float)timeDelta * AlphaLerpStep));
            }
        }
        protected override Func<IRenderable, ITransformation> DefaultStyle
        {
            get
            {
                return
                    ctrl => new PredicateTransformation(
                            expirationPredicate: control =>
                            control.RenderParameters.Transparency == DefaultAlpha,
                            transformer: (timeDelta, control) =>
                            control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                                    .Approach(DefaultAlpha
                                                                              , (float)timeDelta * AlphaLerpStep));
            }
        }

        protected override Func<IRenderable, ITransformation> HoverStyle
        {
            get
            {
                return
                    ctrl => new PredicateTransformation(
                            expirationPredicate: control =>
                            control.RenderParameters.Transparency == HoverAlpha,
                            transformer: (timeDelta, control) =>
                            control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                                    .Approach(HoverAlpha
                                                                             , (float)timeDelta * AlphaLerpStep));
            }
        }

        protected override Func<IRenderable, ITransformation> ClickedStyle
        {
            get
            {
                return
                    ctrl => new NoTransformation();
            }
        }

        #endregion

        public override void Render(IRenderable renderable, SpriteBatch batch)
        {
            return;
        }

    }
}
