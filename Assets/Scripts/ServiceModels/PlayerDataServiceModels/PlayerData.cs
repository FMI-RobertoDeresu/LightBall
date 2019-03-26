using System;

namespace Assets.Scripts.ServiceModels.PlayerDataServiceModels
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData()
        {
            MaxPlayedLevel = -1;
            LevelsBestScore = new int [99];
            LevelsStars = new int [99];
        }

        public int MaxPlayedLevel { get; set; }
        public int[] LevelsBestScore { get; set; }
        public int[] LevelsStars { get; set; }
    }
}