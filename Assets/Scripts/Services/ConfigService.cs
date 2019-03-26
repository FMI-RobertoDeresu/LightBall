using System.IO;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ConfigService : SingletonServiceBase<ConfigService>
    {
        private StagesConfig _stagesConfig;

        protected ConfigService() { }

        public StagesConfig StagesConfig => _stagesConfig ?? ParseStages();

        public static ConfigService Create()
        {
            return CreateInstance();
        }

        private StagesConfig ParseStages()
        {
            var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
            var stagesFileContent = File.ReadAllText(stagesPath);
            _stagesConfig = JsonConvert.DeserializeObject<StagesConfig>(stagesFileContent);
            return _stagesConfig;
        }
    }
}