using TMPro;
using UnityEngine;

namespace Assets.Scripts.Modules.Menu
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

        public void DisplayAsDisabled()
        {
            var shapeText = shapeGo.GetComponent<TextMeshProUGUI>();
            shapeText.color += Color.black * -0.5f;

            var fillText = fillGo.GetComponent<TextMeshProUGUI>();
            fillText.color += Color.black * -0.5f;
        }
    }
}