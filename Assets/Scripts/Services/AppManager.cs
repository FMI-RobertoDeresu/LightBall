using UnityEngine;

namespace Assets.Scripts.Services
{
    public class AppManager : SingletonServiceBase<AppManager>
    {
        private ConfigService _configService;
        private SceneLoader _sceneLoader;
        private PrefabsService _prefabsService;
        private AppContext _appContext;
        private PlayerDataService _playerDataService;

        protected AppManager() { }

        public ConfigService ConfigService => _configService ?? (_configService = ConfigService.Create());
        public SceneLoader SceneLoader => _sceneLoader ?? (_sceneLoader = SceneLoader.Create());
        public PrefabsService PrefabsService => _prefabsService;
        public AppContext AppContext => _appContext ?? (_appContext = AppContext.Create());
        public PlayerDataService PlayerData => _playerDataService ?? (_playerDataService = PlayerDataService.Create());

        public static AppManager Create(GameObject[] prefabs)
        {
            var instance = CreateInstance();
            instance._prefabsService = PrefabsService.Create(prefabs);
            return instance;
        }
    }
}