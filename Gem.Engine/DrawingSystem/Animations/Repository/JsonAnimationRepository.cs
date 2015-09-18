using System;
using System.Collections.Generic;
using Gem.AI.Promises;
using Newtonsoft.Json;
using System.IO;

namespace Gem.DrawingSystem.Animations.Repository
{
    public class JsonAnimationRepository : IAnimationRepository
    {
        private readonly string localPath;
        private readonly Action<string, string> fileWriter;
        private readonly Func<string, string> fileReader;
        private readonly Func<string[]> animationFiles;

        public JsonAnimationRepository(string localPath)
        {
            this.localPath = localPath;
            fileWriter = (json, fileName) => File.WriteAllText($"{localPath}/{fileName}{Extensions.Animation}", json);
            fileReader = fileName => File.ReadAllText(fileName);
            animationFiles =()=> Directory.GetFiles(localPath, $"*{Extensions.Animation}");
        }

        public JsonAnimationRepository(string localPath,
            Action<string, string> fileWriter,
            Func<string, string> fileReader,
            Func<string[]> animationFiles)
        {
            this.localPath = localPath;
            this.fileWriter = fileWriter;
            this.fileReader = fileReader;
            this.animationFiles = animationFiles;
        }


        public IPromise<IEnumerable<AnimationStripSettings>> LoadAll()
        {
            var animationSettings = new List<AnimationStripSettings>();
            try
            {
                foreach (var fileName in animationFiles())
                {
                    animationSettings.Add(JsonConvert.
                        DeserializeObject<AnimationStripSettings>(fileReader(fileName)));
                }
            }
            catch (Exception ex)
            {
                return Promise<IEnumerable<AnimationStripSettings>>.Rejected(ex);
            }
            return Promise<IEnumerable<AnimationStripSettings>>.Resolved(animationSettings);
        }

        public IPromise Save(AnimationStripSettings settings)
        {
            try
            {
                Save(settings, settings.Name);
            }
            catch (Exception ex)
            {
                return Promise.Rejected(ex);
            }
            return Promise.Resolved();
        }

        private void Save(AnimationStripSettings settings, string name)
        {
            var json = JsonConvert.SerializeObject(settings);
            fileWriter(json, name);
        }

        public IPromise SaveRange(IEnumerable<AnimationStripSettings> settings)
        {
            try
            {
                foreach (var setting in settings)
                {
                    Save(setting, setting.Name);
                }
            }
            catch (Exception ex)
            {
                return Promise.Rejected(ex);
            }
            return Promise.Resolved();
        }
    }
}
