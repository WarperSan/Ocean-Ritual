using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSound : MonoBehaviour
{

    [SerializeField] AudioClip MainMusique;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(MainMusique, SoundType.Music, 0.5f, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
