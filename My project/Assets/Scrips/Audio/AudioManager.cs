using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

     
    private float _musicVolume;
    [Range(0,1)]
    [SerializeField] float musicVolume;
    private float _sfxVolume;
    [Range(0, 1)]
    [SerializeField] float sfxVolume;
    
    




    private static AudioSource musicAudioSource;
    private static AudioSource sfxAudioSourse;

    private static AudioManager _instance;

    public static AudioManager instance
    {
        get 
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<AudioManager>();

               
                if (_instance != null)
                {
                    var gameMusic = new GameObject("Music");
                    gameMusic.AddComponent<AudioSource>();
                    musicAudioSource = gameMusic.GetComponent<AudioSource>();
                    var gameSfx = new GameObject("Sfx");
                    gameSfx.AddComponent<AudioSource>();
                    sfxAudioSourse = gameSfx.GetComponent<AudioSource>();

                }

                GameObject game0;
                if (_instance == null)
                {
                    game0 = new GameObject("AudioManager");
                    game0.AddComponent<AudioManager>();
                    _instance = game0.GetComponent<AudioManager>();
                    
                }

                if (_instance != null)
                {
                    var gameMusic = new GameObject("Music");
                    gameMusic.AddComponent<AudioSource>();
                    musicAudioSource = gameMusic.GetComponent<AudioSource>();
                    gameMusic.transform.parent = _instance.gameObject.transform;
                    var gameSfx = new GameObject("Sfx");
                    gameSfx.AddComponent<AudioSource>();

                    gameSfx.transform.parent = _instance.gameObject.transform;
                    sfxAudioSourse = gameSfx.GetComponent<AudioSource>();
                    DontDestroyOnLoad(_instance.gameObject);

                }
            }
            return _instance;

        }
    }


    public void PlayASfx(AudioClip audioClip) 
    {
        sfxAudioSourse.PlayOneShot(audioClip);
    }


    public void PlayMusic(AudioClip audioClip)
    {
        if (musicAudioSource.clip != audioClip)
        {
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }

    private void Update()
    {
        if (musicVolume != _musicVolume)
        {
            _musicVolume = musicVolume;
            musicAudioSource.volume = musicVolume;
        }

        if (sfxVolume != _sfxVolume) 
        {
            _sfxVolume = sfxVolume;
            sfxAudioSourse.volume = musicVolume;
        }
    }
}
