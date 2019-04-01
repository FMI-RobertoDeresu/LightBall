using System.IO;
using System.Linq;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ConfigService : SingletonServiceBase<ConfigService>
    {
        private LevelsConfig _levelsConfig;

        protected ConfigService() { }

        public LevelsConfig LevelsConfig => _levelsConfig ?? ParseLevels();

        public static ConfigService Create()
        {
            return CreateInstance();
        }

        private LevelsConfig ParseLevels()
        {
            var levelsFileContent = Resources.Load<TextAsset>("Levels").ToString();
            _levelsConfig = JsonConvert.DeserializeObject<LevelsConfig>(levelsFileContent);

            foreach (var levelInfo in _levelsConfig.Levels)
                levelInfo.Road.Items = levelInfo.Road.Items.OrderBy(x => x.Position.Value).ToArray();
                
            return _levelsConfig;
        }
    }
}