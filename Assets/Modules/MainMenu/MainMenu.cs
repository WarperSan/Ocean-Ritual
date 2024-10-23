using DhafinFawwaz.AnimationUILib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AnimationUI playAnimation;

    public void PlayButton()
    {
        playAnimation.Play();
    }

    public void ExitButton() 
    {
        Application.Quit();
    }
}
