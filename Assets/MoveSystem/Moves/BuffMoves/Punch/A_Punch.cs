using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Punch : MonoBehaviour
{
    [SerializeField] public PunchAnimation moveAnimation;

    public void PlayAnimation(Vector3 endPoint, List<DamageLog> damageLog, List<PushLog> pushLog)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new PunchAnimationProperties(endPoint, damageLog, pushLog));
    }


    private void End(EntityAnimation<PunchAnimationProperties> obj)
    {

        

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}