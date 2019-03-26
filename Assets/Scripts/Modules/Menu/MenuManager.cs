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
        public GameObject stagesListContentGo;
        public GameObject[] prefabs;

        private void Start()
        {
            var stagesConfig = AppManager.Instance.ConfigService.StagesConfig;
            var playerData = AppManager.Instance.PlayerData.Data;

            var levels = new List<LevelItem>();
            for (var index = 0; index < stagesConfig.Stages.Length; index++)
            {
                var stageConfig = stagesConfig.Stages[index];
                var levelItem = new LevelItem
                {
                    Name = stageConfig.Name,
                    Available = index <= playerData.MaxPlayedLevel + 1,
                    Stars = playerData.LevelsStars[index]
                };
                levels.Add(levelItem);
            }

            var stageItemPrefab = prefabs.First(x => x.name == "MenuStageItem");
            foreach (var level in levels)
            {
                var stageIndex = levels.IndexOf(level);
                var menuItemGo = Instantiate(stageItemPrefab, stagesListContentGo.transform);

                var rectTransform = menuItemGo.transform.GetComponent<RectTransform>();
                rectTransform.position = rectTransform.position + Vector3.down * 80 * stageIndex;

                var menuItem = rectTransform.GetComponent<Level>();
                menuItem.Show(stageIndex, level);
            }

            var rectTransformContent = stagesListContentGo.GetComponent<RectTransform>();
            var newHeight = Math.Max(0, levels.Count * 80 - rectTransformContent.rect.height);
            rectTransformContent.sizeDelta = new Vector2(rectTransformContent.sizeDelta.x, newHeight);

            AppManager.Instance.SceneLoader.SceneIsReady();
        }
    }
}