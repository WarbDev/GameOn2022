using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Punch : MonoBehaviour
{
    [SerializeField] public PunchAnimation moveAnimation;

    public void PlayAnimation(Vector3 endPoint, List<DamageLog> damageLog, List<PushLog> pushLog, IAnimatable enemy, List<Action> scyth)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new PunchAnimationProperties(endPoint, damageLog, pushLog, enemy, scyth));
    }


    private void End(EntityAnimation<PunchAnimationProperties> obj)
    {

        

        moveAnimation.AnimationFinished -= End;

        StartCoroutine(destroy());

        IEnumerator destroy()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }

    


}