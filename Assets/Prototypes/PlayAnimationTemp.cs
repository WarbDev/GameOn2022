using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationTemp : MonoBehaviour
{

    [SerializeField] SpriteAnimation animationn;
    [SerializeField] SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        SpriteAnimationProperties prop = new SpriteAnimationProperties(render);
        animationn.Play(prop);
    }

}
