using EntityModule;

public class BoatEntity : Entity
{
    /// <inheritdoc/>
    protected override void OnStart() => TargetGeneral.Instance.BoatTarget = this.transform;

    /// <inheritdoc/>
    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        this.gameObject.SetActive(false);
    }
}
