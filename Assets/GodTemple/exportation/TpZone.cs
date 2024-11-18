using ControllerModule.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class TpZone : MonoBehaviour
{
    private GameObject player; // Référence au GameObject Player
    [SerializeField] private Transform zoneTp; // Zone de téléportation

    private void Start()
    {
        // Trouve le GameObject avec le tag "Player" au démarrage
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a été trouvé !");
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
