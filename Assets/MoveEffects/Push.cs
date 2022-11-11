using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public PushLog Push(Location direction, int force);
}

public class PushLog
{
    MoveLog moveLog;
}