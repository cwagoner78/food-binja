using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _biteSounds;
    [SerializeField] private AudioClip _bombSound;
    [SerializeField] private AudioClip[] _skullSounds;
    [SerializeField] private AudioClip[] _fartSounds;
    private AudioSource _source;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayBiteSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 1;
        int index = Random.Range(0, _biteSounds.Length);
        _source.PlayOneShot(_biteSounds[index]);
    }

    public void PlayBombSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 0.4f;
        _source.PlayOneShot(_bombSound);
    }

    public void PlaySkullSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 2f;
        int index = Random.Range(0, _skullSounds.Length);
        _source.PlayOneShot(_skullSounds[index]);
    }

    public void PlayFartSound()
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        _source.volume = 3f;
        int index = Random.Range(0, _skullSounds.Length);
        _source.PlayOneShot(_fartSounds[index]);
    }
}
