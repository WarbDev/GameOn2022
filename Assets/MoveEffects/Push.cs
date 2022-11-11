using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public MoveLog Push(Location direction, int force);
}
