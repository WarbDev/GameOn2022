using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TurnPlans : MonoBehaviour
{
    // Pairs of players and whether or not they're able to use their turn.
    [Header("DEBUGGING ONLY: DO NOT MODIFY")]
    [SerializeField] List<PlayerTurnComponent> playerTurns = new();
    public List<PlayerTurnComponent> PlayerTurns { get => playerTurns; }

    /// <summary>
    /// Retrieves collection of all PlayerTurnComponents after initializing them.
    /// </summary>
    public List<PlayerTurnComponent> InitializePlanners()
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
