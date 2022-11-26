using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunActionButton : MonoBehaviour, IBroadcastMove
{
    [SerializeField] List<Move> moves;

    public event Action<Move> Broadcast;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Broadcast?.Invoke(moves[0]);
        }
    }
}
