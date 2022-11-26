using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


// Checks and announces when no more players need to complete their turns.
public class PendingTurns : MonoBehaviour
{
    [SerializeField] List<PlayerTurnComponent> turnPlanners;
    [SerializeField] bool isTracking = false;
    public event Action AllTurnsComplete;  // This event is called when there are no more players than can make their turn.

    /// <summary>
    /// Invokes AllTurnsComplete after all turn planners no longer have a turn pending.
    /// </summary>
    public void TrackPendingTurns(List<PlayerTurnComponent> planners)
    {
        turnPlanners = planners;
        if (isTracking)
        {
            throw new Exception("Is already tracking!");
        }

        isTracking = true;
        StartCoroutine(CheckIfAllPlayersFinished());
    }

    public List<PlayerTurnComponent> PlayersPendingTurns()
    {
        return turnPlanners.Where(x => x.IsTurnPending()).ToList();
    }

    IEnumerator CheckIfAllPlayersFinished()
    {
        // Keep checking every .2 seconds until all players cannot make a turn.
        while(!turnPlanners.All(turn => turn.IsTurnPending()))
        {
            yield return new WaitForSeconds(.1f);
        }
        isTracking = false;
        AllTurnsComplete?.Invoke();
    }
}
