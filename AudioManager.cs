using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds = null;

    // Need Improvement
    [SerializeField]
    private AudioMixer mixer = null;
    private AudioMixerGroup[] groups;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        // Need Improvement
        groups = mixer.FindMatchingGroups("Master");
        
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            // Need Improvement
            if (sound.bgm == true)
            {
                sound.source.outputAudioMixerGroup = groups[1];
            }
            else
            {
                sound.source.outputAudioMixerGroup = groups[2];
            }
        }
    }

    private void Start()
    {
        Play("Sea");
    }
    
    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        Instance = null;
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        
        s.source.Stop();
    }

    public void SetMute()
    {
        foreach (var sound in sounds)
        {
            sound.source.volume = 0f;
            sound.source.pitch = 0f;
        }
    }

    public void SetUnMute()
    {
        foreach (var sound in sounds)
        {
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }
}
