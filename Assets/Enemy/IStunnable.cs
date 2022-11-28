using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    public int StunDuration { get; }
    public void AddStun(int rounds);
}
