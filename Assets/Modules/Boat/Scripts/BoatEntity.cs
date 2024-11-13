using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEntity : Entity
{
    [SerializeField] GameObject gameOverScreen;

    private AudioSource sourceSound;
    [SerializeField] AudioClip attackSound;

    protected override void OnStart()
    {
        sourceSound = GetComponent<AudioSource>();
        sourceSound.clip = attackSound;
    }
    protected override void OnDeath(float overDamage)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
        this.gameObject.SetActive(false);
    }

    protected override void OnPostAttack(Projectile source)
    {
        sourceSound.Play();
    }
}
