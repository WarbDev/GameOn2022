using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Spin : MonoBehaviour
{
    [SerializeField] public SpinAnimation moveAnimation;

    public void PlayAnimation(Player player, FaceCamera facer)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new SpinAnimationProperties(player, facer));
    }


    private void End(EntityAnimation<SpinAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}