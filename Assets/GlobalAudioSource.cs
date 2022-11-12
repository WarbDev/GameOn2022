using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioSource : MonoBehaviour
{
    public static GlobalAudioSource Instance;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, 1f);
    }
}
