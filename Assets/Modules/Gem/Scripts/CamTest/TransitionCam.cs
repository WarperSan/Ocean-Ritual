using Unity.Cinemachine;
using UnityEngine;
using UtilsModule;

public class TransitionCam : Singleton<TransitionCam>
{
    public CinemachineVirtualCamera camA;
    public CinemachineVirtualCamera camB;

    [SerializeField]
    private bool switchToCamA;

    [SerializeField]
    private bool switchToCamB;

    public void Update()
    {
        if (switchToCamA)
            SwitchToCamA();

        if (switchToCamB)
            SwitchToCamB();
    }

    public void SwitchToCamB()
    {
        camA.Priority = 0;
        camB.Priority = 10;
    }

    public void SwitchToCamA()
    {
        //Debug.Log("a");
        camA.Priority = 10;
        camB.Priority = 0;
    }

    protected override bool DestroyOnLoad => true;
}