using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public abstract void DoMove(Player player);
}
