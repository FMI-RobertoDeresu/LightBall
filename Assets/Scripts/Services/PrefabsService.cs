using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PrefabsService : SingletonServiceBase<PrefabsService>
    {
        private GameObject[] _prefabs;

        protected PrefabsService() { }

        public GameObject LoaderPrefab => _prefabs.First(x => x.name == "Loader");
        public GameObject LoaderFadeInPrefab => _prefabs.First(x => x.name == "LoaderFadeIn");

        public static PrefabsService Create(GameObject[] prefabs)
        {
            var instance = CreateInstance();
            instance._prefabs = prefabs;
            return instance;
        }
    }
}