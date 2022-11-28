using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CurrentlyPlanningPlayer : MonoBehaviour
{
    public static PlayerTurnComponent CurrentlyPlanning;
    public event Action TriedAction;

    public void TrackPlanningPlayer(List<PlayerTurnComponent> planners, Func<bool> requirement)
    {
        StartCoroutine(Routine(planners, requirement));
    }


    IEnumerator Routine(List<PlayerTurnComponent> players, Func<bool> requirement)
    {
        List<PlayerTurnComponent> planners = new List<PlayerTurnComponent>(players);
        foreach(var planner in planners)
        {
            planner.StateChanged += OnStateChange;
            planner.TriedAction += InvokeTriedAction;
        }
        while (requirement())
        {
            
            yield return null;
        }
        
        void OnStateChange(PlayerTurnComponent planner, PlayerTurnComponent.PLAN_STATE state)
        {
            if (CurrentlyPlanning == planner)
            {
                if (!PlayerTurnComponent.ActiveStates.Contains(state))
                {
                    CurrentlyPlanning = null;
                }
            }

            if (PlayerTurnComponent.ActiveStates.Contains(state))
            {
                CurrentlyPlanningPlayer.CurrentlyPlanning = planner;
            }
        }
        foreach (var planner in planners)
        {
            planner.StateChanged -= OnStateChange;
            planner.TriedAction -= InvokeTriedAction;
        }

        void InvokeTriedAction()
        {
            TriedAction?.Invoke();
        }
    }
}
