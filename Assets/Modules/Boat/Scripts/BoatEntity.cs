using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEntity : Entity
{
    [SerializeField] GameObject gameOverScreen;
    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        this.gameObject.SetActive(false);
    }
}
