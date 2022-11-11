using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Wind : MonoBehaviour
{

    [SerializeField] WindAnimation windAnimation;
    // Start is called before the first frame update
    public void PlayAnimation()
    {
        windAnimation.AnimationFinished -= End;
        windAnimation.AnimationFinished += End;
        windAnimation.Play(new WindAnimationProperties());
    }

    private void End(EntityAnimation<WindAnimationProperties> obj)
    {

        //ADD PUSH ENEMY ANIMATION


        windAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
