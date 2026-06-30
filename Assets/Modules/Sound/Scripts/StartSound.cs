using UnityEngine;

public class StartSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip MainMusique;

    // Start is called before the first frame update
    private void Start() => SoundManager.Instance.PlaySound(MainMusique,
        SoundType.Music,
        0.2f,
        true);

    // Update is called once per frame
    private void Update()
    {
    }
}