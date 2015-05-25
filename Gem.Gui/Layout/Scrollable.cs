using Gem.Gui.Alignment;
using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gem.Gui.Layout
{
    [Layout("Scrollable")]
    public class Scrollable : AControl
    {

        #region Fields

        private readonly ListView listView;
        private readonly ControlCamera area;

        #endregion

        #region Ctor

        internal Scrollable(Texture2D texture,
                        AlignmentContext alignment,
                        Orientation orientation,
                        Region region,
                        ReadOnlyCollection<AControl> controls)
            : base(texture, region, new NoStyle())
        {
            this.listView = new ListView(texture, alignment, orientation, region, controls);

            area = new ControlCamera(region.Position,
                                      region.Frame,
                                      new Rectangle(region.Frame.X, region.Frame.Y,
                                                   (int)listView.VirtualSize.X, (int)listView.VirtualSize.Y));

            region.SetPositionTransformer(position => area.Position - position);

            throw new NotImplementedException();
        }


        #endregion

        #region Scrollable Members and Properties


        #endregion

        #region Alignment

        public AlignmentContext ControlAlignment { get; private set; }

        public override void Align(Region viewPort)
        {
            //base.Align(viewPort);    
            listView.Align(viewPort);
        }

        public override void Scale(Vector2 scale)
        {
            listView.Scale(scale);

            base.Scale(scale);
        }


        #endregion

        #region AControl Members

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            listView.Update(deltaTime);
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);
            listView.Render(batch);
        }

        public override IEnumerable<AControl> Entries()
        {
            return listView.Entries();
        }

        #endregion
    }
}
