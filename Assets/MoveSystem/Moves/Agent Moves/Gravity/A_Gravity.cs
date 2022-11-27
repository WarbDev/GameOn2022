using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Gravity : MonoBehaviour
{
    [SerializeField] public GravityAnimation moveAnimation;

    public void PlayAnimation()
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new GravityAnimationProperties());
    }


    private void End(EntityAnimation<GravityAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}

