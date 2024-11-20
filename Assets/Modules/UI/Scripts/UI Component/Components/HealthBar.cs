using DhafinFawwaz.AnimationUILib;
using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Components
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthBar;

        [SerializeField]
        private AnimationUI showAnimation;

        [SerializeField]
        private AnimationUI hideAnimation;

        public void InitializeBar(Entity boss)
        {
            this.healthBar.minValue = 0;
            this.healthBar.maxValue = boss.MaxHealth;
            this.healthBar.value = boss.Health;

        }

        public void UpdateBar(Entity boss)
        {
            StartCoroutine(AnimateHealthChange(boss.Health));
        }

        private IEnumerator AnimateHealthChange(float targetHealth)
        {
            float currentHealth = healthBar.value;
            float elapsedTime = 0f;
            float duration = 0.5f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                healthBar.value = Mathf.Lerp(currentHealth, targetHealth, elapsedTime / duration);
                yield return null;
            }

            healthBar.value = targetHealth;
        }

        public void Show()
        {
            this.showAnimation.Play();
        }

        public void Hide()
        {
            this.hideAnimation.Play();
        }
    }
}
