using ControllerModule.Controllers;
using UnityEngine;
using System.Threading.Tasks;

public class TpZone : MonoBehaviour
{
    private GameObject player; // R�f�rence au GameObject Player

    [SerializeField]
    private Transform zoneTp; // Zone de t�l�portation

    [SerializeField]
    private bool enter;

    [SerializeField]
    private bool exit;

    [SerializeField]
    private AudioClip MusicEnter;

    [SerializeField]
    private AudioClip Exit;

    [SerializeField]
    private AudioClip TpSound;

    private void Start()
    {
        // Trouve le GameObject avec le tag "Player" au d�marrage
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a �t� trouv� !");
    }

    public void SoundFunction()
    {
        Debug.Log("aaa");
        SoundManager.Instance.PlaySound(TpSound, SoundType.Ambient, 0.2f);

        if (TpSound == null)
        {
            Debug.LogError("TpSound n'est pas assign� !");
            return;
        }

        if (enter)
            SoundManager.Instance.PlaySound(MusicEnter,
                SoundType.Music,
                0.5f,
                true);
        else if (exit)
        {
            SoundManager.Instance.StopSound(SoundType.Music);

            SoundManager.Instance.PlaySound(Exit,
                SoundType.Music,
                0.3f,
                true);
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            PlayerController test = player.GetComponent<PlayerController>();

            if (test != null)
            {
                // D�sactiver le script
                test.enabled = false;
                SoundFunction();
                // T�l�porter le joueur
                player.transform.position = zoneTp.position;

                // Attendre 0.1 seconde avant de r�activer le script
                await Task.Delay(100);

                // R�activer le script
                test.enabled = true;
            }
            else
                Debug.LogWarning("Le script PlayerController est introuvable sur le GameObject Player.");
        }
    }
}