using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Highlight : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public Color Color { get => spriteRenderer.color; set => spriteRenderer.color = value; }
}
