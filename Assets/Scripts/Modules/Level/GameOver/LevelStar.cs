﻿using UnityEngine;

namespace Assets.Scripts.Modules.Level.GameOver
{
    public class LevelStar : MonoBehaviour
    {
        public GameObject shapeGo;
        public GameObject fillGo;

        public void Display(bool show, bool fill)
        {
            shapeGo.SetActive(show);
            fillGo.SetActive(fill);
        }
    }
}