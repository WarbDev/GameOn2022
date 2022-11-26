using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnPlanningInput : MonoBehaviour
{
    [SerializeField] GameObject movementButton;
    [SerializeField] GameObject actionButton;

    IBroadcastVoid movementSelect;
    IBroadcastMove actionSelect;

    public event Action MovementSelected;
    public event Action<Move> ActionSelected;

    void Start()
    {
        movementSelect = movementButton.GetComponent<IBroadcastVoid>();
        actionSelect = actionButton.GetComponent<IBroadcastMove>();

        movementSelect.Broadcast += (() => MovementSelected?.Invoke());
        actionSelect.Broadcast += (x => ActionSelected?.Invoke(x));
    }


}
