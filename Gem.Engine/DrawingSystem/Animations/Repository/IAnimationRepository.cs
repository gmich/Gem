using Gem.AI.Promises;
using System.Collections.Generic;
using Gem.Repositories;

namespace Gem.DrawingSystem.Animations
{
    public interface IAnimationRepository : IRepository<AnimationStripSettings>
    {
    }

    public partial class Extensions
    {
        public const string Animation = ".animation";
    }
}
