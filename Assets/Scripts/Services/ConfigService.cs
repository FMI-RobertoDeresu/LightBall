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
            var levelsPath = Path.Combine(Application.dataPath, "Config/Levels.json");
            var levelsFileContent = File.ReadAllText(levelsPath);
            _levelsConfig = JsonConvert.DeserializeObject<LevelsConfig>(levelsFileContent);

            foreach (var levelInfo in _levelsConfig.Levels)
                levelInfo.Road.Items = levelInfo.Road.Items.OrderBy(x => x.Position.Value).ToArray();
                
            return _levelsConfig;
        }
    }
}