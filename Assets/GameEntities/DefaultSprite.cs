using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color defaultColor;
    public void ResetToDefaultColor()
    {
        spriteRenderer.color = defaultColor;
    }
}
