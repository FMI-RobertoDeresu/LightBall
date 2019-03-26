using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules
{
    public class LoaderManager : MonoBehaviour
    {
        private Action _afterFade;

        public Animator animator;
        public Slider slider;
        public Text sliderText;

        public void FadeIn(Action afterFade)
        {
            _afterFade = afterFade;
            animator.SetTrigger("FadeIn");
        }

        protected void OnFadeComplete()
        {
            _afterFade?.Invoke();
            Destroy(gameObject);
        }

        public void UpdateProgress(float progress)
        {
            slider.value = progress;
            sliderText.text = $"{Convert.ToInt16(progress * 100)}%";
        }
    }
}