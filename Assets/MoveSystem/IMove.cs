using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    public void DoMove(Player player);
    //you're using a lot of classes to make the move work
}

public interface IDamage
{
    public float Damage { get; }
}