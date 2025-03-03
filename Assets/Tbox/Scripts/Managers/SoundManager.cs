using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sounds[] sounds;
    [SerializeField] private AudioSource soundEffectsSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Music")]
    public List<string> musicTracks; // Lista de nombres de canciones

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

        PlayRandomMusic();
    }

    public void PlaySound(string name)
    {
        Sounds sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            if (sound.randomPitch)
            {
                soundEffectsSource.pitch = Random.Range(sound.rPitchMinValue, sound.rPitchMaxValue);
            }
            else
            {
                soundEffectsSource.pitch = sound.pitch;
            }

            soundEffectsSource.clip = sound.clip;
            soundEffectsSource.volume = sound.volume;
            soundEffectsSource.loop = sound.loop;
            soundEffectsSource.Play();
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

    public void PlayMusic(string name)
    {
        Sounds sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume; // Asegúrate de que el volumen se está configurando aquí
            musicSource.loop = sound.loop;
Debug.Log("Playing music: " + name + " with volume: " + sound.volume);
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music: " + name + " not found!");
        }
    }

    public void PlayRandomMusic()
    {
        if (musicTracks == null || musicTracks.Count == 0)
        {
            Debug.LogWarning("Music track list is empty or null!");
            return;
        }

        string randomMusicName = musicTracks[Random.Range(0, musicTracks.Count)];
        PlayMusic(randomMusicName);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3,3)] public float pitch = 1;
    public float rPitchMinValue = 0.8f;
    public float rPitchMaxValue = 1.2f;
    public bool loop = false;
    public bool randomPitch = false;
}
