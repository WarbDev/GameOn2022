using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_Ice : MonoBehaviour
{
    [SerializeField] public IceAnimation moveAnimation;

    public void PlayAnimation(List<TerrainBase> terrainLog)
    {
        moveAnimation.AnimationFinished -= End;
        moveAnimation.AnimationFinished += End;
        moveAnimation.Play(new IceAnimationProperties(terrainLog));
    }


    private void End(EntityAnimation<IceAnimationProperties> obj)
    {

        moveAnimation.AnimationFinished -= End;
        Destroy(gameObject);
    }
}
