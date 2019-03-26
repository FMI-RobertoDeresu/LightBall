using System;
using System.IO;
using Assets.Scripts.Models.Config.Stages;
using Newtonsoft.Json;

namespace Assets.Scripts.Services
{
    public class ConfigService
    {
        private void Test()
        {
            try
            {
                var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
                var stagesFileContent = File.ReadAllText(stagesPath);
                var stagesConfig = JsonConvert.DeserializeObject<StagesConfig>(stagesFileContent);
                _stageInfo = stagesConfig.Stages[0];

                RenderStage(_stageInfo);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}