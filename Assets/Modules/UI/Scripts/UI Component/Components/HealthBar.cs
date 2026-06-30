using DhafinFawwaz.AnimationUILib;
using EntityModule;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Components
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private AnimationUI showAnimation;

        [SerializeField]
        private AnimationUI hideAnimation;

        public void InitializeBar(Entity boss)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = boss.MaxHealth;
            healthBar.value = boss.Health;
        }

        public void UpdateBar(Entity boss) => StartCoroutine(AnimateHealthChange(boss.Health));

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

        public void Show() => showAnimation.Play();

        public void Hide() => hideAnimation.Play();
    }
}