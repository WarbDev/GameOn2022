using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/SpriteSet")]
public class SpriteSet : ScriptableObject
{
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    public List<Sprite> Sprites { get => sprites; }
}
