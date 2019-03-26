using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.ServiceModels.PlayerDataServiceModels;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PlayerDataService : SingletonServiceBase<PlayerDataService>
    {
        private string _filePath;
        private PlayerData _data;

        protected PlayerDataService() { }

        public PlayerData Data => _data;

        public static PlayerDataService Create()
        {
            var instance = CreateInstance();
            instance._filePath = Path.Combine(Application.persistentDataPath, "playerData.dat");
            instance.Load();
            return instance;
        }

        private void Load()
        {
            if (!File.Exists(_filePath))
            {
                _data = new PlayerData();
                return;
            }

            using (var file = File.Open(_filePath, FileMode.Open))
            {
                var bf = new BinaryFormatter();
                _data = (PlayerData)bf.Deserialize(file);
            }
        }

        public void SaveChanges()
        {
            using (var file = File.Open(_filePath, FileMode.OpenOrCreate))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, _data);
            }
        }
    }
}