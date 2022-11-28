using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Barricade: MonoBehaviour
{
    [SerializeField] public BarricadeAnimation moveAnimation;

    public void PlayAnimation()
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new BarricadeAnimationProperties());
    }


    private void End(EntityAnimation<BarricadeAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}

