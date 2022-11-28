using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class A_Blaze : MonoBehaviour
{
    [SerializeField] public BlazeAnimation moveAnimation;

    public void PlayAnimation()
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new BlazeAnimationProperties());
    }


    private void End(EntityAnimation<BlazeAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
