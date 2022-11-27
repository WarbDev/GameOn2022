using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementButton : MonoBehaviour, IBroadcastVoid
{
    public event Action Broadcast;

    public void Clicked()
    {
        Broadcast?.Invoke();
    }
}