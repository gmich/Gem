using System;
using System.Collections.Generic;
using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using Gem.Gui.Utilities;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Styles
{
    public class TransparentControlStyle : IRenderStyle
    {

        #region Fields

        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();

        #endregion

        public TransparentControlStyle()
        {
            this.AssignDefaultValues();
        }

        private void Flush()
        {
            foreach (var transformation in activeTransformations)
            {
                transformation.Dispose();
            }
        }

        #region Properties

        [DefaultValue(1.0f)]
        public float FocusAlpha { get; set; }

        [DefaultValue(0.8f)]
        public float HoverAlpha { get; set; }

        [DefaultValue(0.6f)]
        public float DefaultAlpha { get; set; }

        [DefaultValue(2.0f)]
        public float AlphaLerpStep { get; set; }

        #endregion

        #region Style

        public void Focus(AControl styeControl)
        {
            Flush();

            activeTransformations.Add(
                styeControl.AddTransformation(new PredicateTransformation(
                    expirationPredicate: control =>
                        control.RenderParameters.Transparency == FocusAlpha,
                        transformer: (timeDelta, control) =>
                        control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                                .Approach(FocusAlpha
                                                                         ,(float)timeDelta * AlphaLerpStep))));

        }

        public void Default(AControl styeControl)
        {
            Flush();

            activeTransformations.Add(
            styeControl.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == DefaultAlpha,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                            .Approach(DefaultAlpha
                                                                      ,(float)timeDelta * AlphaLerpStep))));
        }

        public void Hover(AControl styeControl)
        {
            Flush();

            activeTransformations.Add(
            styeControl.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == HoverAlpha,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                            .Approach(HoverAlpha
                                                                     ,(float)timeDelta * AlphaLerpStep))));
        }

        public void Clicked(AControl styeControl)
        {
            return;
        }

        #endregion

        public void Render(SpriteBatch batch)
        {
            return;
        }

    }
}
