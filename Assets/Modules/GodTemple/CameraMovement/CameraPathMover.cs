using UnityEngine;
using System.Collections;
using InteractModule;
using ControllerModule.Controllers;
using Unity.Cinemachine;

public class CameraPathMover : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CinemachineDollyCart dollyCart; // R�f�rence au Dolly Cart

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera; // R�f�rence � la cam�ra Cinemachine

    [SerializeField]
    private float speed = 5f; // Vitesse de d�placement

    [SerializeField]
    private float startDelay = 1f; // D�lai avant que la piste commence

    [SerializeField]
    private float endPriority = 1; // Priorit� apr�s la fin

    [SerializeField]
    private float activePriority = 20; // Priorit� active pendant le trajet

    private GameObject player; // R�f�rence au GameObject Player
    private bool isMoving;

    public InteractionAsset InteractionAsset => null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a �t� trouv� !");

        // Assurez-vous que la cam�ra commence avec la priorit� basse
        if (virtualCamera != null)
            virtualCamera.Priority = (int)endPriority;
    }

    private void Update()
    {
        if (isMoving && dollyCart != null)
        {
            // D�placement de la cam�ra sur le chemin
            dollyCart.m_Position += speed * Time.deltaTime;

            // V�rifie si la cam�ra a atteint la fin du chemin
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
                StopMovement();
        }
    }

    public void OnClick() => StartCoroutine(StartCameraMovement());

    private IEnumerator StartCameraMovement()
    {
        // Augmenter la priorit� de la cam�ra
        if (virtualCamera != null)
        {
            PlayerController script = player.GetComponent<PlayerController>();
            script.enabled = false;
            virtualCamera.Priority = (int)activePriority;
        }

        // Attendre avant de commencer le mouvement
        yield return new WaitForSeconds(startDelay);

        // Commence � bouger la cam�ra
        isMoving = true;
    }

    private void StopMovement()
    {
        // R�duire la priorit� de la cam�ra
        if (virtualCamera != null)
            virtualCamera.Priority = (int)endPriority;
        PlayerController script = player.GetComponent<PlayerController>();
        script.enabled = true;
        // Arr�ter le mouvement
        isMoving = false;

        // R�initialiser la position du chariot si n�cessaire
        dollyCart.m_Position = 0;
    }
}