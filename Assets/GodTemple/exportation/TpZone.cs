using ControllerModule.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class TpZone : MonoBehaviour
{

   [SerializeField] Transform zoneTp;
    [SerializeField] GameObject player;
    [SerializeField] bool TpEnterGoodZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private async void OnTriggerEnter(Collider other)
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
            Debug.LogWarning("PlayerController script introuvable sur l'objet.");
        }
    }

}
