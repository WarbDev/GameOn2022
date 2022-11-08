using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    public void DoMove();
}

public interface IDamage
{
    public float Damage { get; set; }
}