using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnPlanningInput : MonoBehaviour
{
    [SerializeField] MovementButton movementButton;
    [SerializeField] MovePanel actionButton;
    [SerializeField] SkipPlayerTurn skipPlayerTurn;

    IBroadcastVoid movementSelect;
    IBroadcastMove actionSelect;

    public event Action MovementSelected;
    public event Action<Move> ActionSelected;
    public event Action SkippingTurn;

    void Start()
    {
        movementSelect = movementButton.GetComponent<IBroadcastVoid>();
        actionSelect = actionButton.GetComponent<IBroadcastMove>();
        skipPlayerTurn.RequestedPlayerSkip += () => SkippingTurn?.Invoke();
        movementSelect.Broadcast += (() => MovementSelected?.Invoke());
        actionSelect.Broadcast += (x => ActionSelected?.Invoke(x));
    }


}
