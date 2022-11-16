using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Fireball : MonoBehaviour
{
    [SerializeField] ProjectileFireballAnimation projectileFireball;
    [SerializeField] BoomFireballAnimation boomFireball;
    List<DamageLog> log;

    public void PlayAnimation(Vector3 endPoint, List<DamageLog> log)
    {
        this.log = log;
        projectileFireball.AnimationFinished -= Boom;
        projectileFireball.AnimationFinished += Boom;
        projectileFireball.Play(new PFireballAnimationProperties(transform.position, endPoint));
    }

    private void Boom(EntityAnimation<PFireballAnimationProperties> pfire)
    {
        projectileFireball.AnimationFinished -= Boom;

        boomFireball.AnimationFinished -= End;
        boomFireball.AnimationFinished += End;
        projectileFireball.enabled = false;

        boomFireball.Play(new BFireballAnimationProperties());
    }

    private void End(EntityAnimation<BFireballAnimationProperties> obj)
    {
        //log[0].Target.GameObject.GetComponent<AnimatableEntity>();
        foreach(DamageLog damaged in log)
        {
            damaged.Target.GameObject.GetComponent<AnimatableEntity>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }


        boomFireball.AnimationFinished -= End;
        Destroy(gameObject);
    }
}