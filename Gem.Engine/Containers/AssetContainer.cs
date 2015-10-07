using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Gem.Engine.Containers
{
    public class AssetContainer<TAsset>
        where TAsset : class
    {

        private readonly Dictionary<string, TAsset> assets = new Dictionary<string, TAsset>();
        public ContentManager Content { get; }

        public AssetContainer(ContentManager content)
        {
            Content = content;
        }

        public bool Add(string id, Func<ContentManager, TAsset> assetRetriever)
        {
            if (assets.ContainsKey(id))
            {
                return false;
            }
            assets.Add(id, assetRetriever(Content));
            return true;
        }

        public bool Add(string id, string path)
        {
            if (assets.ContainsKey(id))
            {
                return false;
            }
            assets.Add(id, Content.Load<TAsset>(path));
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
