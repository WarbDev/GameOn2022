using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnMovement : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] MovementPlanningInput input;
    Dictionary<Location, MovementOption> movementOptions;

    /// <summary>
    /// Returns false if no movement was actually made.
    /// </summary>
    public event Action<bool> DidMovement;

    void Start()
    {
        input.SelectedLocation += OnSelectedLocation;
        input.PlanningCancelled += CancelPlanning;
    }

    public void PlanMovement()
    {
        movementOptions = GenerateMovementOptions();
        input.StartRetrieving(movementOptions);
    }

    void OnSelectedLocation(Location location)
    {
        if (movementOptions[location] == MovementOption.SWAP)
        {
            Player secondPlayer;
            LocationUtility.TryGetPlayer(location, out secondPlayer);
            Swap(secondPlayer);
        }
        else if (movementOptions[location] == MovementOption.FREE)
        {
            MoveTo(location);
        }
        else CancelPlanning();
    }

    public void CancelPlanning()
    {
        input.StopRetrieving();
        DidMovement?.Invoke(false);
    }

    void Swap(Player playerToSwapTo)
    {
        GameMap.SwapPlayers(player, playerToSwapTo);
        DidMovement?.Invoke(true);
    }

    void MoveTo(Location location)
    {
        GameMap.MovePlayer(player, location);
        DidMovement?.Invoke(true);
    }

    public Dictionary<Location, MovementOption> GenerateMovementOptions()
    {
        Dictionary<Location, MovementOption> options = new();
        foreach (var loc in LocationUtility.GetColumn(0))
        {
            options.Add(loc, GetMovementOption(loc));
        }
        return options;

        MovementOption GetMovementOption(Location location)
        {
            if (Math.Abs(location.Y - player.Location.Y) > 1)
            {
                return MovementOption.OBSTRUCTED;
            }

            if (LocationUtility.TryGetPlayer(location, out Player p))
            {
                if (p == player)
                {
                    return MovementOption.OBSTRUCTED;
                }
                else return MovementOption.SWAP;
            }
            if (obstructionChecker.IsObstructedBy(location))
            {
                return MovementOption.OBSTRUCTED;
            }

            return MovementOption.FREE;
        }
    }
}

public enum MovementOption
{
    OBSTRUCTED,
    FREE,
    SWAP
}