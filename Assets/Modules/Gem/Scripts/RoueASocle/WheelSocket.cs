using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSocket : MonoBehaviour
{
   // private float timeSinceLastRotation = 0f;
    public float rotationInterval = 10f;
    public float rotationDuration = 2f;
    public float rotationAmount = 45f;

    private bool isRotating = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float rotationTimeElapsed = 0f;
   // private bool rotateLeft = true;
    [SerializeField] bool dejaTourner = true;
    public float ticIntensity = 2f; 
    public float ticDuration = 0.5f; 
    private bool isTicActive = false;
    private float ticTimeElapsed = 0f;
    [SerializeField] MouseWheelManager mouseWheelManager;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!isRotating && !isTicActive)
        {
            if (dejaTourner)
            {
                StartRotationUP();
                dejaTourner = false;
            }
        }

        if (isRotating)
        {
            RotateWheel();
        }

        if (isTicActive)
        {
            CercleTic();
        }
    }
    public bool IsRotating()
    {
        return isRotating;
    }
   public void StartRotationUP()
    {
        mouseWheelManager.StartRotation();
        isRotating = true;
        mouseWheelManager.GetNextSocle();
         rotationTimeElapsed = 0f;
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(rotationAmount, 0f, 0f);
    }

    public void StartRotationDown()
    {
        mouseWheelManager.StartRotation();
        isRotating = true;
        mouseWheelManager.GetPreviewSocle();
        rotationTimeElapsed = 0f;
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(-rotationAmount, 0f, 0f);
    }

    void RotateWheel()
    {
        rotationTimeElapsed += Time.deltaTime;
        float t = rotationTimeElapsed / rotationDuration;

        transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

        if (rotationTimeElapsed >= rotationDuration)
        {
            isRotating = false;
            transform.rotation = targetRotation;
            StartTicWheel();
        }
    }

    void StartTicWheel()
    {
        isTicActive = true;
        ticTimeElapsed = 0f;
    }

    void CercleTic()
    {
        ticTimeElapsed += Time.deltaTime;
        float t = ticTimeElapsed / ticDuration;

       
        float oscillation = Mathf.Sin(t * Mathf.PI * 2) * ticIntensity;
        transform.rotation = targetRotation * Quaternion.Euler(oscillation, 0f, 0f);

        if (ticTimeElapsed >= ticDuration)
        {
            isTicActive = false;
            transform.rotation = targetRotation;
            mouseWheelManager.EndRotation();
        }
    }
}
