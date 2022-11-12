using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Taunt : MonoBehaviour
{
    [SerializeField] TauntAnimation animate;

    public void PlayAnimation(List<PushLog> log)
    {
        animate.AnimationFinished -= End;
        animate.AnimationFinished += End;
        animate.Play(new TauntAnimationProperties());
    }

    private void End(EntityAnimation<TauntAnimationProperties> obj)
    {

        //ADD TAUNTED ENEMY ANIMATION


        animate.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
