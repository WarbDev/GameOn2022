using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable: ITickable
{
    public int StunDuration { get; }
    public void AddStun(int rounds);
}

public interface ITickable
{
    public void OnRoundTick();
}