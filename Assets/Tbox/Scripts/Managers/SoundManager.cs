using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sounds[] sounds;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        Sounds sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        
    }

    public void PlayRandomSound(List<string> soundNames)
    {
        if (soundNames == null || soundNames.Count == 0)
        {
            Debug.LogWarning("Sound list is empty or null!");
            return;
        }

        string randomSoundName = soundNames[Random.Range(0, soundNames.Count)];
        PlaySound(randomSoundName);
    }

}

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3,3)] public float pitch = 1;
    public bool loop = false;
}
