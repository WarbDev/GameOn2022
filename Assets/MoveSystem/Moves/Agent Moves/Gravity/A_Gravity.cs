using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Gravity : MonoBehaviour
{
    [SerializeField] public GravityAnimation moveAnimation;

    public void PlayAnimation(Vector3 endPoint)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new GravityAnimationProperties(transform.position, endPoint));
    }


    private void End(EntityAnimation<GravityAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}

