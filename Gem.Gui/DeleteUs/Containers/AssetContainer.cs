using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Assets
{
    public class AssetContainer<TAsset>
        where TAsset : class
    {

        private readonly Dictionary<string, TAsset> assets = new Dictionary<string, TAsset>();
        private readonly ContentManager content;

        public AssetContainer(ContentManager content)
        {
            this.content = content;
        }

        public void AddAsset(string id, Func<ContentManager, TAsset> assetRetriever)
        {
            assets.Add(id, assetRetriever(content));
        }

        public void AddAsset(string id, string assetPath)
        {
            assets.Add(id, content.Load<TAsset>(assetPath));
        }

        public TAsset this[string id]
        {
            get
            {
                return assets.ContainsKey(id) ?
                       assets[id] : null;
            }
        }
    }
}
