using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Gun : MonoBehaviour
{
    [SerializeField] public GunAnimation moveAnimation;
    List<DamageLog> log;

    public void PlayAnimation(Vector3 endPoint, List<DamageLog> log)
    {
        this.log = log;
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new GunAnimationProperties(transform.position, endPoint));
    }


    private void End(EntityAnimation<GunAnimationProperties> obj)
    {

        foreach (DamageLog damaged in log)
        {
            damaged.Target.Entity.GetComponent<IAnimatable>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }


        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}

