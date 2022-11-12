using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadAnimation : EntityAnimation<BadAnimationProperties>
{
    public override event Action<EntityAnimation<BadAnimationProperties>> AnimationFinished;
    [SerializeField] AudioClip clip;

    public static BadAnimation Instance;

    public override void Pause()
    {
        
    }

    public override void Play(BadAnimationProperties animationProperties)
    {
        GlobalAudioSource.Instance.Play(clip);
    }

    public override void Unpause()
    {
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class BadAnimationProperties : IAnimationProperties
{
    
}