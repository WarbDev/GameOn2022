using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunMovementButton : MonoBehaviour, IBroadcastVoid
{

    public event Action Broadcast;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Broadcast?.Invoke();
        }
    }
}
