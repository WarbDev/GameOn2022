using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Gravity : MonoBehaviour
{
    [SerializeField] public GravityAnimation moveAnimation;

    public void PlayAnimation(Vector3 endPoint, List<Enemy> enemies, List<PushLog> pushLog)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new GravityAnimationProperties(enemies, pushLog, transform.position, endPoint));
    }


    private void End(EntityAnimation<GravityAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}

