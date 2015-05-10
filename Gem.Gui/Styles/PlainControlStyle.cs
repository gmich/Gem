using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Core.Styles
{
    public class PlainControlStyle : IRenderStyle
    {

        #region Fields

        private readonly AControl control;
        private readonly float transparencyTransition = 0.1f;
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();

        #endregion

        public PlainControlStyle(AControl control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            this.control = control;
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
            Flush();
            float focusTransparency = 1.0f;

            activeTransformations.Add(
                this.control.AddTransformation(new PredicateTransformation(
                    expirationPredicate: control =>
                        control.RenderParameters.Transparency == focusTransparency,
                        transformer: (timeDelta, control) =>
                        control.RenderParameters.Transparency = MathHelper.Min(focusTransparency,
                                                                   control.RenderParameters.Transparency += (float)timeDelta * transparencyTransition))));
        }

        public void Default()
        {
            Flush();
            float defaultTransparency = 0.5f;

            activeTransformations.Add(
            this.control.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == defaultTransparency,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = MathHelper.Min(defaultTransparency,
                                                               control.RenderParameters.Transparency += (float)timeDelta * transparencyTransition))));
        }

        public void Hover()
        {
            Flush();
            float hoverTransparency = 0.7f;

            activeTransformations.Add(
            this.control.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == hoverTransparency,
                    transformer: (timeDelta, control) =>
                    control.RenderParameters.Transparency = MathHelper.Min(hoverTransparency,
                                                               control.RenderParameters.Transparency -= (float)timeDelta * transparencyTransition))));
        }
        
        public void Clicked()
        {
            return;
        }

        #endregion

    }
}
