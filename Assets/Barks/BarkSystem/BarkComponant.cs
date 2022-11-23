using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkComponant : MonoBehaviour
{

    [SerializeField] List<AudioClip> SpeakClips;
    [SerializeField] List<AudioClip> HurtClips;
    [SerializeField] AudioClip DieClip;

    public void PlaySpeakBark()
    {
        int randNumber = Random.Range(1, SpeakClips.Count);
        GlobalAudioSource.Instance.Play(SpeakClips[randNumber]);
    }

    public void PlayHurtBark()
    {
        int randNumber = Random.Range(1, HurtClips.Count);
        GlobalAudioSource.Instance.Play(HurtClips[randNumber]);
    }

    public void PlayDieBark()
    {
        GlobalAudioSource.Instance.Play(DieClip);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
