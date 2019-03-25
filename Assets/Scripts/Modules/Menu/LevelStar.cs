using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Modules.Menu
{
    public class LevelStar : MonoBehaviour
    {
        private bool _ready;
        private bool _show;
        private bool _fill;

        public GameObject shapeGo;
        public GameObject fillGo;

        public void BeforeStart(bool show, bool fill)
        {
            _show = show;
            _fill = fill;
            _ready = true;
        }

        private void Start()
        {
            StartCoroutine(Display());
        }

        private IEnumerator Display()
        {
            yield return new WaitUntil(() => _ready);
            shapeGo.SetActive(_show);
            fillGo.SetActive(_fill);
        }
    }
}