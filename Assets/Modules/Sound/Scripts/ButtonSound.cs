using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] string soundName;
    public void OnClick()
    {
        SoundManager.Instance.PlaySound(soundName,SoundType.UI);
    }
}
