using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnKeyDown : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] AnimatableEntity animateable;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            animateable.PlayAnimation(ANIMATION_ID.ENTITY_JUMP, new JumpAnimationProperties(transform.position, transform.position + Vector3.left));
        }
    }
}
