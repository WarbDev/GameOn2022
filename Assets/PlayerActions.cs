using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all actions this player character has access to, and if they're on cooldown.
/// </summary>
public class PlayerActions : EntityComponent
{
    [SerializeField] List<Move> moves = new();
    [SerializeField] GenericDictionary<Move, int> movesOnCooldown;


    public List<Move> AllMoves { get => moves; }

    public void TickCooldown()
    {
        foreach (var mC in movesOnCooldown)
        {
            movesOnCooldown[mC.Key] = Mathf.Max(0, mC.Value - 1);
        }
    }

    public bool CanDo(Move move)
    {
        return moves.Contains(move) && !IsOnCoolDown(move);
    }

    public bool IsOnCoolDown(Move move)
    {
        return movesOnCooldown.ContainsKey(move) && movesOnCooldown[move] > 0;
    }

    public int Cooldown(Move move)
    {
        return movesOnCooldown[move];
    }


}
