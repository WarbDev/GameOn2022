using System;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public abstract event Action<bool> MoveCompleted;

    public abstract void DoMove(Player player);
}
