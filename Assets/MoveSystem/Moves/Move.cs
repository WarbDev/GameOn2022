using System;
using UnityEngine;

public abstract class Move : ScriptableObject
{
    public abstract event Action<bool> MoveCompleted;

    public abstract void DoMove(Player player);
    [SerializeField] string myName;
    [SerializeField] string tooltip;
    [SerializeField] int cooldown;

    public string Name { get => myName; }
    public string Tooltip { get => tooltip; }
    public int Cooldown { get => cooldown; }
}
