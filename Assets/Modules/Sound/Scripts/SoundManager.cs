using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilsModule;

public class SoundManager : Singleton<SoundManager>
{
    
    AudioSource uiSoundSource;
    AudioSource ambientSource;
    AudioSource musicSource;

    private string uiSoundPath = "Assets/Resources/Sound/UI/";
    private string ambientPath = "Assets/Resources/Sound/Ambience/";
    private string musicPath = "Assets/Resources/Sound/Music/";

    private bool uiPlaying = false;
    private bool ambientPlaying = false;
    private bool musicPlaying = false;

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
    public void PlaySound(AudioClip sound, SoundType type, bool looped = false, bool waitForEnd = false)
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
                uiSoundSource.loop = looped;
                uiSoundSource.Play();
                break;
        }
    }

    /// <summary>
    /// Plays a sound on the given sound type's track
    /// </summary>
    public void PlaySound(string soundPath, SoundType type, bool looped = false, bool waitForEnd = false)
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
                    

                musicSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(musicPath + soundPath, typeof(AudioClip));
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
                    

                ambientSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(ambientPath + soundPath, typeof(AudioClip));
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

                uiSoundSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(uiSoundPath + soundPath, typeof(AudioClip));
                uiSoundSource.loop = looped;
                uiSoundSource.Play();
                break;
        }
    }

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
    
    
}

public enum SoundType
{
    UI, Ambient, Music
}
