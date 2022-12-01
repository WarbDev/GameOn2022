using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] SpriteRenderer render;

    public void PlayAnimation()
    {
        if (gameObject.transform.position.x < 0)
        {
            render.flipX = true;
        }
        spriteAnimation.Play(new SpriteAnimationProperties(render));
    }
}
