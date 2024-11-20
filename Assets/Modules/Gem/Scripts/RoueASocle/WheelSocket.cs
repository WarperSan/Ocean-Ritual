using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSocket : MonoBehaviour
{
   // private float timeSinceLastRotation = 0f;
    public float rotationInterval = 10f;
    public float rotationDuration = 2f;
    public float rotationAmount = 45f;
    [SerializeField] GameObject RotateObject;
    private bool isRotating = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float rotationTimeElapsed = 0f;
   // private bool rotateLeft = true;
   
    public float ticIntensity = 2f; 
    public float ticDuration = 0.5f; 
    private bool isTicActive = false;
    private float ticTimeElapsed = 0f;
    [SerializeField] MouseWheelManager mouseWheelManager;
    [SerializeField] AudioClip MusicSpin;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
      

        if (isRotating&& !isTicActive)
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
    [SerializeField] AudioClip popSound;
    public void StartRotationUP()
    {
        playSound();
        if (mouseWheelManager.IsAlone())
        {
            targetRotation= RotateObject.transform.rotation ;
            StartTicWheel();
            return;
        }
        mouseWheelManager.StartRotation();
        isRotating = true;
        mouseWheelManager.GetNextSocle();
        int skip = mouseWheelManager.getNumberforRotation();
         rotationTimeElapsed = 0f;
        initialRotation = RotateObject.transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(rotationAmount + (skip * rotationAmount), 0f, 0f);
    }
    public void playSound()
    {
       
        SoundManager.Instance.PlaySound(popSound, SoundType.Ambient, 1f);
    }
    public void StartRotationDown()
    {
        playSound();
        if (mouseWheelManager.IsAlone())
        {
            targetRotation = RotateObject.transform.rotation;
            StartTicWheel();
            return;
        }
        mouseWheelManager.StartRotation();
        isRotating = true;
        mouseWheelManager.GetPreviewSocle();
        int skip = mouseWheelManager.getNumberforRotation();
        rotationTimeElapsed = 0f;
        initialRotation = RotateObject.transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(-rotationAmount+(skip* -rotationAmount), 0f, 0f);
    }

    void RotateWheel()
    {
        rotationTimeElapsed += Time.deltaTime;
        float t = rotationTimeElapsed / rotationDuration;

        RotateObject.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
        SoundManager.Instance.PlaySound(MusicSpin, SoundType.UI, 1f, true,true);
        if (rotationTimeElapsed >= rotationDuration)
        {
            //isRotating = false;
            RotateObject.transform.rotation = targetRotation;
            StartTicWheel();
        }
    }

    void StartTicWheel()
    {
        mouseWheelManager.StartRotation();
        SoundManager.Instance.StopSound(SoundType.UI);
        isTicActive = true;
        ticTimeElapsed = 0f;
    }

    void CercleTic()
    {
        ticTimeElapsed += Time.deltaTime;
        float t = ticTimeElapsed / ticDuration;

       
        float oscillation = Mathf.Sin(t * Mathf.PI * 2) * ticIntensity;
        RotateObject.transform.rotation = targetRotation * Quaternion.Euler(oscillation, 0f, 0f);

        if (ticTimeElapsed >= ticDuration)
        {
            isRotating = false;
            isTicActive = false;
            RotateObject.transform.rotation = targetRotation;
            mouseWheelManager.EndRotation();
        }
    }
}
