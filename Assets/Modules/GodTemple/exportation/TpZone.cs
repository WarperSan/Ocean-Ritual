using ControllerModule.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UIModule.Components;
public class TpZone : MonoBehaviour
{
    private GameObject player; // Référence au GameObject Player
    [SerializeField] private Transform zoneTp; // Zone de téléportation
    [SerializeField] bool enter = false;
    [SerializeField] bool exit = false;
    [SerializeField] AudioClip MusicEnter ;
    [SerializeField] AudioClip Exit;
    [SerializeField] AudioClip TpSound;
    private void Start()
    {
        // Trouve le GameObject avec le tag "Player" au démarrage
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a été trouvé !");
        }
    }
    public void SoundFunction()
    {
        Debug.Log("aaa");
        SoundManager.Instance.PlaySound(TpSound, SoundType.Ambient, 1f);
        if (TpSound == null)
        {
            Debug.LogError("TpSound n'est pas assigné !");
            return;
        }
        if (enter)
        {
            SoundManager.Instance.PlaySound(MusicEnter,SoundType.Music,0.5f,true);
        }
        else if (exit)
        {
            SoundManager.Instance.StopSound(SoundType.Music);
            SoundManager.Instance.PlaySound(Exit, SoundType.Music, 0.3f, true);
        }

    }

    private async void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            PlayerController test = player.GetComponent<PlayerController>();

            if (test != null)
            {
                // Désactiver le script
                test.enabled = false;
                SoundFunction();
                // Téléporter le joueur
                player.transform.position = zoneTp.position;

                // Attendre 0.1 seconde avant de réactiver le script
                await Task.Delay(100);

                // Réactiver le script
                test.enabled = true;
            }
            else
            {
                Debug.LogWarning("Le script PlayerController est introuvable sur le GameObject Player.");
            }
        }
    }

}
