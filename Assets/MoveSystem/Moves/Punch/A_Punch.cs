using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Punch : MonoBehaviour
{
    [SerializeField] PunchAnimation moveAnimation;
    List<DamageLog> log;

    public void PlayAnimation(Vector3 endPoint, List<DamageLog> log)
    {
        this.log = log;
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new PunchAnimationProperties(endPoint));
    }


    private void End(EntityAnimation<PunchAnimationProperties> obj)
    {

        foreach (DamageLog damaged in log)
        {
            damaged.Target.Entity.GetComponent<AnimatableEntity>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}