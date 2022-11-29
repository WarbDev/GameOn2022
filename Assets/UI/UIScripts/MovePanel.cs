using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour, IBroadcastMove
{
    [SerializeField] List<MoveButton> moveButtons;
    [SerializeField] UIPlayerEvents events;

    public event Action<Move> Broadcast;

    private void Start()
    {
        foreach(var button in moveButtons)
        {
            button.Broadcast += (x => Broadcast?.Invoke(x));
        }
    }
}
