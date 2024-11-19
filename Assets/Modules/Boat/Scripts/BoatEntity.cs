using EntityModule;
using UnityEngine;
using UnityEngine.UI;
public class BoatEntity : Entity
{
    [SerializeField] Slider slider;
    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        this.gameObject.SetActive(false);
    }
    protected override void OnStart()
    {
        base.OnStart();

        if (slider != null)
        {
            // Initialiser le slider avec la santé maximale
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

    protected override void ModifyHeal(Heal heal)
    {
       
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        if (slider != null)
        {
            slider.value = Health; // Met à jour la valeur du slider
        }
    }
}
