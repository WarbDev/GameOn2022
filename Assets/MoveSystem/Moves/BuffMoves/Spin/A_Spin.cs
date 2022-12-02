using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class A_Spin : MonoBehaviour
{
    [SerializeField] public SpinAnimation moveAnimation;

    private List<Action> scyth;
    public void PlayAnimation(Player player, FaceCamera facer, List<PushLog> pushLog, List<DamageLog> damageLog, List<Action> scyth)
    {
        this.scyth = scyth;
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new SpinAnimationProperties(player, facer, pushLog, damageLog));
    }


    private void End(EntityAnimation<SpinAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        

        StartCoroutine(destroy());
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0f);
        

        Destroy(gameObject);
    }
}