using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


// Checks and announces when no more players need to complete their turns.
public class PendingTurns : MonoBehaviour
{
    // Pairs of players and whether or not they're able to use their turn.
    [Header("DEBUGGING ONLY: DO NOT MODIFY")]
    [SerializeField] List<PlayerTurnComponent> playerTurns = new();

    public event Action AllTurnsComplete;  // This event is called when there are no more players than can make their turn.

    public void TrackPendingTurns()
    {
        playerTurns = InitializePlayerTurns();
        StartCoroutine(CheckIfAllPlayersFinished());
    }

    IEnumerator CheckIfAllPlayersFinished()
    {
        // Keep checking every .2 seconds until all players cannot make a turn.
        while(!playerTurns.All(turn => turn.IsTurnPending()))
        {
            yield return new WaitForSeconds(.1f);
        }
        AllTurnsComplete?.Invoke();
    }

    List<PlayerTurnComponent> InitializePlayerTurns()
    {
        List<PlayerTurnComponent> turns = new();
        foreach (Player player in Entities.PlayerCollection.EntitiesSet)
        {
            var turnComponent = player.TurnComponent;
            turns.Add(turnComponent);
            turnComponent.BeginNewRound();
        }
        return turns;
    }

    void OnPlayerDeleted(Player player)
    {
        playerTurns.Remove(player.TurnComponent);
    }
    void OnPlayerAdded(Player player)
    {
        playerTurns.Add(player.TurnComponent);
    }


    private void OnEnable()
    {
        Entities.PlayerCollection.EntityRemoved += OnPlayerDeleted;
        Entities.PlayerCollection.EntityAdded += OnPlayerAdded;
    }

    private void OnDisable()
    {
        Entities.PlayerCollection.EntityRemoved -= OnPlayerDeleted;
        Entities.PlayerCollection.EntityAdded -= OnPlayerAdded;
    }
}
