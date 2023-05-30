using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is NULL:");
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioSource _sniperAudioSource;
    private void Awake()
    {
        _instance = this;
    }

    public void PlayVoice(AudioClip clitToPlay)
    {
        if (clitToPlay.name =="Sniper-shoot")
        {
            _sniperAudioSource.clip = clitToPlay;
            _sniperAudioSource.Play();
        }
        else
        {
            _audioSource.clip = clitToPlay;
            _audioSource.Play();
        }
      
    }
}
