using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoldTheLine.Examples
{
    public class A_ : MonoBehaviour
    {
        [SerializeField] MoveAnimation moveAnimation;

        public void PlayAnimation()
        {
            moveAnimation.AnimationFinished -= End;
            moveAnimation.AnimationFinished += End;
            moveAnimation.Play(new MoveAnimationProperties());
        }


        private void End(EntityAnimation<MoveAnimationProperties> obj)
        {

            moveAnimation.AnimationFinished -= End;
            Destroy(gameObject);
        }
    }
}
