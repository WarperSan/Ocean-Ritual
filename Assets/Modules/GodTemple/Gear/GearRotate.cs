using UnityEngine;

public class GearRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float minRotationSpeed = 10f; // Vitesse minimale

    public float maxRotationSpeed = 50f;   // Vitesse maximale
    public float minRotationDuration = 1f; // Dur�e minimale de rotation dans une direction
    public float maxRotationDuration = 5f; // Dur�e maximale de rotation dans une direction

    [Header("Tic Settings")]
    public float ticIntensity = 5f; // Intensit� de l'oscillation

    public float ticDuration = 0.5f; // Dur�e du Tic

    private float currentRotationSpeed;    // Vitesse actuelle
    private float rotationTimeElapsed;     // Temps �coul� pour la rotation actuelle
    private float currentRotationDuration; // Dur�e actuelle de rotation
    private bool isRotatingLeft = true;    // Direction de rotation

    private bool isTicActive;           // D�termine si un Tic est en cours
    private float ticTimeElapsed;       // Temps �coul� pour le Tic
    private Quaternion initialRotation; // Rotation initiale pour compenser les orientations de d�part
    private Quaternion targetRotation;  // Rotation cible pour le Tic

    private void Start()
    {
        initialRotation = transform.localRotation; // Sauvegarde la rotation initiale locale
        SetRandomRotation();
    }

    private void Update()
    {
        if (isTicActive)
            CercleTic();
        else
            RotateGear();
    }

    private void SetRandomRotation()
    {
        // D�termine une vitesse et une dur�e de rotation al�atoires
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        currentRotationDuration = Random.Range(minRotationDuration, maxRotationDuration);
        isRotatingLeft = Random.value > 0.5f; // 50% de chance de changer de direction
        rotationTimeElapsed = 0f;
    }

    private void RotateGear()
    {
        // Applique la rotation directement sur l'axe X relatif � l'orientation initiale
        float rotationDirection = isRotatingLeft ? -1f : 1f;
        var rotationStep = Quaternion.Euler(0f, rotationDirection * currentRotationSpeed * Time.deltaTime, 0f);
        transform.localRotation *= rotationStep;

        // Incr�mente le temps �coul�
        rotationTimeElapsed += Time.deltaTime;

        // Change de direction ou d�clenche un Tic si la dur�e est atteinte
        if (rotationTimeElapsed >= currentRotationDuration)
        {
            if (Random.value > 0.7f) // 30% de chance de d�clencher un Tic
                StartTic();
            else
                SetRandomRotation(); // Change de direction
        }
    }

    private void StartTic()
    {
        isTicActive = true;
        ticTimeElapsed = 0f;
        targetRotation = transform.localRotation; // Initialise `targetRotation` � la rotation locale actuelle
    }

    private void CercleTic()
    {
        ticTimeElapsed += Time.deltaTime;
        float t = ticTimeElapsed / ticDuration;

        // Calcul de l'oscillation sur l'axe X tout en respectant l'orientation initiale
        float oscillation = Mathf.Sin(t * Mathf.PI * 2) * ticIntensity;
        transform.localRotation = targetRotation * Quaternion.Euler(0f, oscillation, 0f);

        // Arr�te le Tic lorsque sa dur�e est atteinte
        if (ticTimeElapsed >= ticDuration)
        {
            isTicActive = false;
            transform.localRotation = targetRotation; // R�initialise la rotation exacte
            SetRandomRotation();                      // Reprend une rotation normale
        }
    }
}