using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBroadcastVoid
{
    public event Action Broadcast;
}

public interface IBroadcastMove
{
    public event Action<Move> Broadcast;
}

public interface IBroadcastPlayer
{
    public event Action<Player> Broadcast;
}