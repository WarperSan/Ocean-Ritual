using UnityEngine;
using Cinemachine;
using System.Collections;
using InteractModule;
using ControllerModule.Controllers;

public class CameraPathMover : MonoBehaviour, IInteractable
{
    [SerializeField] private CinemachineDollyCart dollyCart; // Référence au Dolly Cart
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Référence à la caméra Cinemachine
    [SerializeField] private float speed = 5f; // Vitesse de déplacement
    [SerializeField] private float startDelay = 1f; // Délai avant que la piste commence
    [SerializeField] private float endPriority = 1; // Priorité après la fin
    [SerializeField] private float activePriority = 20; // Priorité active pendant le trajet
    private GameObject player; // Référence au GameObject Player
    private bool isMoving = false;

    public InteractionAsset InteractionAsset => null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a été trouvé !");
        }
        // Assurez-vous que la caméra commence avec la priorité basse
        if (virtualCamera != null)
        {
            virtualCamera.Priority = (int)endPriority;
        }
    }

    private void Update()
    {
        if (isMoving && dollyCart != null)
        {
            // Déplacement de la caméra sur le chemin
            dollyCart.m_Position += speed * Time.deltaTime;

            // Vérifie si la caméra a atteint la fin du chemin
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {
                StopMovement();
            }
        }
    }

    public void OnClick()
    {
        StartCoroutine(StartCameraMovement());
    }

    private IEnumerator StartCameraMovement()
    {
        // Augmenter la priorité de la caméra
        if (virtualCamera != null)
        {
            PlayerController script = player.GetComponent<PlayerController>();
            script.enabled = false;
            virtualCamera.Priority = (int)activePriority;
        }

        // Attendre avant de commencer le mouvement
        yield return new WaitForSeconds(startDelay);

        // Commence à bouger la caméra
        isMoving = true;
    }

    private void StopMovement()
    {
        // Réduire la priorité de la caméra
        if (virtualCamera != null)
        {
            virtualCamera.Priority = (int)endPriority;
        }
        PlayerController script = player.GetComponent<PlayerController>();
        script.enabled = true;
        // Arrêter le mouvement
        isMoving = false;

        // Réinitialiser la position du chariot si nécessaire
        dollyCart.m_Position = 0;
    }
}
