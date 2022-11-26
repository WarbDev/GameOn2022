using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunMovementButton : MonoBehaviour, IBroadcastVoid
{
    [SerializeField] KeyCode keyCode;
    public event Action Broadcast;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Broadcast?.Invoke();
        }
    }
}
