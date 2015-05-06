using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Core.Styles
{
    public class PlainButtonStyle : IRenderStyle
    {

        #region Fields

        private readonly Button button;
        private readonly float transparencyTransition = 0.1f;
        private readonly List<IDisposable> activeTransformations;

        #endregion

        public PlainButtonStyle(Button button)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button");
            }

            this.button = button;
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
                this.button.AddTransformation(new PredicateTransformation(
                    expirationPredicate: control =>
                        control.RenderParameters.Transparency == focusTransparency,
                    regionTransformer: (timeDelta, renderParams) =>
                        renderParams.Transparency = MathHelper.Min(focusTransparency,
                                                                   renderParams.Transparency += (float)timeDelta * transparencyTransition))));
        }

        public void Default()
        {
            Flush();
            float defaultTransparency = 0.5f;

            activeTransformations.Add(
            this.button.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == defaultTransparency,
                regionTransformer: (timeDelta, renderParams) =>
                    renderParams.Transparency = MathHelper.Min(defaultTransparency,
                                                               renderParams.Transparency += (float)timeDelta * transparencyTransition))));
        }

        public void Hover()
        {
            Flush();
            float hoverTransparency = 0.7f;

            activeTransformations.Add(
            this.button.AddTransformation(new PredicateTransformation(
                expirationPredicate: control =>
                    control.RenderParameters.Transparency == hoverTransparency,
                regionTransformer: (timeDelta, renderParams) =>
                    renderParams.Transparency = MathHelper.Min(hoverTransparency,
                                                               renderParams.Transparency -= (float)timeDelta * transparencyTransition))));
        }
        
        public void Clicked()
        {
            return;
        }

        #endregion

    }
}
