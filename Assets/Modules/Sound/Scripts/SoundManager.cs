using UnityEngine;
using UtilsModule;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource uiSoundSource;
    private AudioSource ambientSource;
    private AudioSource musicSource;

    private string uiSoundPath = "Assets/Resources/Sound/UI/";
    private string ambientPath = "Assets/Resources/Sound/Ambience/";
    private string musicPath = "Assets/Resources/Sound/Music/";

    protected override void OnAwake()
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        uiSoundSource = sources[1];
        ambientSource = sources[0];
        musicSource = sources[2];
        Debug.Log(musicSource.name);
    }

    /// <summary>
    /// Plays a sound on the given sound type's track
    /// </summary>
    public void PlaySound(
        AudioClip sound,
        SoundType type,
        float     volume     = 0.8f,
        bool      looped     = false,
        bool      waitForEnd = false
    )
    {
        switch (type)
        {
            case SoundType.Music:
                if (musicSource.isPlaying)
                {
                    if (waitForEnd)
                        break;

                    StopSound(SoundType.Music);
                }

                musicSource.clip = sound;
                musicSource.volume = volume;
                musicSource.loop = looped;
                musicSource.Play();
                break;
            case SoundType.Ambient:
                if (ambientSource.isPlaying)
                {
                    if (waitForEnd)
                        break;

                    StopSound(SoundType.Ambient);
                }

                ambientSource.clip = sound;
                ambientSource.volume = volume;
                ambientSource.loop = looped;
                ambientSource.Play();
                break;
            case SoundType.UI:
                if (uiSoundSource.isPlaying)
                {
                    if (waitForEnd)
                        break;

                    StopSound(SoundType.UI);
                }

                uiSoundSource.clip = sound;
                uiSoundSource.volume = volume;
                uiSoundSource.loop = looped;
                uiSoundSource.Play();
                break;
        }
    }

    /// <summary>
    /// Plays a sound on the given sound type's track
    /// </summary>
    //public void PlaySound(string soundPath, SoundType type,float volume = 0.8f, bool looped = false, bool waitForEnd = false)
    //{
    //    switch (type)
    //    {
    //        case SoundType.Music:

    //            if (musicSource.isPlaying)
    //            {
    //                if (waitForEnd)
    //                    break;
    //                StopSound(SoundType.Music);
    //            }

    //            musicSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(musicPath + soundPath, typeof(AudioClip));
    //            musicSource.volume = volume;
    //            musicSource.loop = looped;
    //            musicSource.Play();
    //            break;
    //        case SoundType.Ambient:
    //            if (ambientSource.isPlaying)
    //            {
    //                if (waitForEnd)
    //                    break;
    //                StopSound(SoundType.Ambient);
    //            }

    //            ambientSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(ambientPath + soundPath, typeof(AudioClip));
    //            ambientSource.volume = volume;
    //            ambientSource.loop = looped;
    //            ambientSource.Play();
    //            break;
    //        case SoundType.UI:
    //            if (uiSoundSource.isPlaying)
    //            {
    //                if (waitForEnd) 
    //                    break;
    //                StopSound(SoundType.UI);
    //            }

    //            uiSoundSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(uiSoundPath + soundPath, typeof(AudioClip));
    //            uiSoundSource.volume = volume;
    //            uiSoundSource.loop = looped;
    //            uiSoundSource.Play();
    //            break;
    //    }
    //}

    /// <summary>
    /// Stops a sound on the given sound type's track
    /// </summary>
    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Music:
                musicSource.Stop();
                break;
            case SoundType.Ambient:
                ambientSource.Stop();
                break;
            case SoundType.UI:
                uiSoundSource.Stop();
                break;
        }
    }

    protected override bool DestroyOnLoad => true;
    protected override bool KeepParent    => false;
}

public enum SoundType
{
    UI, Ambient, Music,
}