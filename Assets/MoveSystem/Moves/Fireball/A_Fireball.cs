using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Fireball : MonoBehaviour
{
    [SerializeField] ProjectileFireballAnimation projectileFireball;
    [SerializeField] BoomFireballAnimation boomFireball;

    public void PlayAnimation(Vector3 endPoint)
    {
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

        //ADD DAMAGE ENEMY ANIMATION


        boomFireball.AnimationFinished -= End;
        Destroy(gameObject);
    }
}