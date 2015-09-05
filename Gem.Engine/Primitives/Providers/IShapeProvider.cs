using System.Collections.Generic;

namespace Gem.Engine.Primitives.Providers
{
    public interface IShapeProvider
    {
        IList<IShape> Load();
        bool Save(IList<IShape> shapes);
    }
}