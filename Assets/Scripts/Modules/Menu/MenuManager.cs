using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Menu;
using UnityEngine;

namespace Assets.Scripts.Modules.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject stagesListContentGo;
        public GameObject[] prefabs;

        private void Start()
        {
            var stages = new List<StageItem>
            {
                new StageItem {Name = "Lvl. 1", Available = true, Played = true, Stars = 3},
                new StageItem {Name = "Lvl. 2", Available = true, Played = true, Stars = 2},
                new StageItem {Name = "Lvl. 3", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 4", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 5", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 6", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 7", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 8", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 9", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 10", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 11", Available = true, Played = true, Stars = 1},
                new StageItem {Name = "Lvl. 12", Available = true, Played = true, Stars = 1}
            };

            var stageItemPrefab = prefabs.First(x => x.name == "MenuStageItem");
            foreach (var stage in stages)
            {
                var stageIndex = stages.IndexOf(stage);
                var menuItemGo = Instantiate(stageItemPrefab, stagesListContentGo.transform);

                var rectTransform = menuItemGo.transform.GetComponent<RectTransform>();
                rectTransform.position = rectTransform.position + Vector3.down * 80 * stageIndex;

                var menuItem = rectTransform.GetComponent<Level>();
                menuItem.Show(stageIndex, stage.Name, stage.Stars);
            }

            var rectTransformContent = stagesListContentGo.GetComponent<RectTransform>();
            var newHeight = Math.Max(rectTransformContent.rect.height,
                stages.Count * 80 - rectTransformContent.rect.height);
            rectTransformContent.sizeDelta = new Vector2(rectTransformContent.sizeDelta.x, newHeight);
        }
    }
}