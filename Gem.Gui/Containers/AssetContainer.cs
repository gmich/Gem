using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Containers
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

        public bool Add(string id, Func<ContentManager, TAsset> assetRetriever)
        {
            if (assets.ContainsKey(id))
            {
                return false;
            }
            assets.Add(id, assetRetriever(content));
            return true;
        }

        public bool Add(string id, string path)
        {
            if (assets.ContainsKey(id))
            {
                return false;
            }
            assets.Add(id, content.Load<TAsset>(path));
            return true;
        }

        public bool Remove(string id)
        {
            return assets.Remove(id);
        }

        public TAsset this[string id]
        {
            get
            {
                if (assets.ContainsKey(id))
                {
                    return assets[id];
                }
                else
                {
                    throw new ArgumentException(id);
                }
            }
        }
    }
}
