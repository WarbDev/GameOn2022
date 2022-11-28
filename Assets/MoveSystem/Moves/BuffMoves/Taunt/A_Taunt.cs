using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Taunt : MonoBehaviour
{
    [SerializeField] public TauntAnimation animate;

    public void PlayAnimation(List<PushLog> log)
    {
        animate.AnimationFinished -= End;
        animate.AnimationFinished += End;
        animate.Play(new TauntAnimationProperties());
    }

    private void End(EntityAnimation<TauntAnimationProperties> obj)
    {

        animate.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
