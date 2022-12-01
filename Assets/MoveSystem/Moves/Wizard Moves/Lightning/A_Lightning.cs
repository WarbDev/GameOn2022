using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Lightning : MonoBehaviour
{
    [SerializeField] public LightningAnimation moveAnimation;
    List<DamageLog> log;
    [SerializeField] SpriteRenderer render;


    public void Flip()
    {
        render.flipX = true;
    }


    public void PlayAnimation(List<Location> area, List<DamageLog> logs)
    {
        log = logs;
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new LightningAnimationProperties(area));
    }


    private void End(EntityAnimation<LightningAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}