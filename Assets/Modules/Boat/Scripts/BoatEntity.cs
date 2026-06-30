using EntityModule;
using UnityEngine;
using UnityEngine.UI;

public class BoatEntity : Entity
{
    [SerializeField]
    private Slider slider;

    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        gameObject.SetActive(false);
    }

    protected override void OnStart()
    {
        base.OnStart();

        TargetGeneral.Instance.BoatTarget = transform;

        if (slider != null)
        {
            // Initialiser le slider avec la sant� maximale
            slider.maxValue = Health;
            slider.value = Health;
        }
    }

    public override bool TakeDamage => true;

    protected override void OnPostAttack(Projectile source = null)
    {
        base.OnPostAttack(source);
        UpdateHealthSlider();
    }

    protected override void ModifyHeal(Heal heal) => UpdateHealthSlider();

    private void UpdateHealthSlider()
    {
        if (slider != null)
            slider.value = Health; // Met � jour la valeur du slider
    }
}