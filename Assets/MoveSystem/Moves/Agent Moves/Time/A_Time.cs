using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class A_Time : MonoBehaviour
{
    [SerializeField] public TimeAnimation moveAnimation;

    public void PlayAnimation(TimebombTerrain bomb, Vector3 endPoint)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new TimeAnimationProperties(bomb, endPoint));
    }


    private void End(EntityAnimation<TimeAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
