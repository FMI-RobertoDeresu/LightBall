using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ModuleModels.Menu;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Modules.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject levelsListContentGo;
        public GameObject[] prefabs;

        private void Start()
        {
            var levelsConfig = AppManager.Instance.ConfigService.LevelsConfig;
            var playerData = AppManager.Instance.PlayerData.Data;

            var levels = new List<LevelItem>();
            for (var index = 0; index < levelsConfig.Levels.Length; index++)
            {
                var levelConfig = levelsConfig.Levels[index];
                var levelItem = new LevelItem
                {
                    Name = levelConfig.Name,
                    Available = index <= playerData.MaxPlayedLevel + 1,
                    Stars = playerData.LevelsStars[index]
                };
                levels.Add(levelItem);
            }

            var levelItemPrefab = prefabs.First(x => x.name == "MenuLevelItem");
            foreach (var level in levels)
            {
                var levelIndex = levels.IndexOf(level);
                var menuItemGo = Instantiate(levelItemPrefab, levelsListContentGo.transform);

                var rectTransform = menuItemGo.transform.GetComponent<RectTransform>();
                var rect = rectTransform.rect;
                var pos = new Vector3(0, -(rect.height + 10) * levelIndex - rect.height / 2);
                rectTransform.anchoredPosition = pos;

                var menuItem = rectTransform.GetComponent<Level>();
                menuItem.Show(levelIndex, level);
            }

            var rectTransformContent = levelsListContentGo.GetComponent<RectTransform>();
            var newHeight = Math.Max(0, levels.Count * 80 - rectTransformContent.rect.height);
            rectTransformContent.sizeDelta = new Vector2(rectTransformContent.sizeDelta.x, newHeight);

            AppManager.Instance.SceneLoader.SceneIsReady();
        }
    }
}