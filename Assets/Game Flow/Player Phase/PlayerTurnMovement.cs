using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnMovement : MonoBehaviour
{
    [SerializeField] bool canDoMovement;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    public bool CanDoMovement { get => canDoMovement; }

    

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
            if (LocationUtility.HasPlayer(location))
            {
                return MovementOption.SWAP;
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