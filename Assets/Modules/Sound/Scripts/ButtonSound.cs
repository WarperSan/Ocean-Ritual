using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundName;

    public void OnClick() => SoundManager.Instance.PlaySound(soundName, SoundType.UI);
}