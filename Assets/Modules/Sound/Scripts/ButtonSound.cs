using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip soundName;
    public void OnClick()
    {
        SoundManager.Instance.PlaySound(soundName,SoundType.UI);
    }
}
