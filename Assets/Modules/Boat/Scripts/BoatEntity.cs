using EntityModule;

public class BoatEntity : Entity
{
    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        this.gameObject.SetActive(false);
    }
}
