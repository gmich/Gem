using Gem.AI.Promises;
using Gem.DrawingSystem.Animations;
using Gem.DrawingSystem.Animations.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Gem.Repositories;

namespace Gem.Engine.Tests.Repositories
{
    [TestClass]
    public class AnimationRepositoryTests
    {
        private IAnimationRepository animationRepository;

        [TestMethod]
        public void AnimationRepository()
        {
            string json = null;
            animationRepository = new JsonAnimationRepository("path",
                (generatedJson, fileName) => json = generatedJson,
                fileName => json,
                () => new[] { "path" });

            AnimationStripSettings settings = new AnimationStripSettings(10, 10, 0, 0, "test", 20d, true, null, 1, 10);
            AnimationStripSettings loadedSettings = default(AnimationStripSettings);

            animationRepository.ExecuteAsync(rep => rep.Save(settings))
                               .Then(() =>
                                    animationRepository.LoadAll())
                               .Then(jsonSettings =>
                                    loadedSettings = jsonSettings.First());

            Assert.AreEqual(settings.Name, loadedSettings.Name);


        }


    }
}
