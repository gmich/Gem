using System;
using System.Collections.Generic;
using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using Gem.Gui.Utilities;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Gui.Core.Styles
{
    public class PlainControlStyle : IRenderStyle
    {

        #region Fields

        private readonly AControl control;
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();

        #endregion

        public PlainControlStyle(AControl control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            this.control = control;
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

        [DefaultValue(0.7f)]
        public float HoverAlpha { get; set; }

        [DefaultValue(0.5f)]
        public float DefaultAlpha { get; set; }

        [DefaultValue(1.0f)]
        public float AlphaLerpStep { get; set; }

        #endregion

        #region Style

        public void Focus()
        {
            Flush();

            activeTransformations.Add(
                this.control.AddTransformation(new PredicateTransformation(
                    expirationPredicate: control =>
                        control.RenderParameters.Transparency == FocusAlpha,
                        transformer: (timeDelta, control) =>
                        control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                                .Approach(FocusAlpha
                                                                         , (float)timeDelta * AlphaLerpStep))));

        }

        public void Default()
        {
            Flush();

            activeTransformations.Add(
            this.control.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == DefaultAlpha,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                            .Approach(DefaultAlpha
                                                                      , (float)timeDelta * AlphaLerpStep))));
        }

        public void Hover()
        {
            Flush();

            activeTransformations.Add(
            this.control.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == HoverAlpha,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = control.RenderParameters.Transparency
                                                            .Approach(HoverAlpha
                                                                     , (float)timeDelta * AlphaLerpStep))));
        }

        public void Clicked()
        {
            return;
        }

        #endregion

    }
}
