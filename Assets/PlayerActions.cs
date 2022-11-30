using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Contains all actions this player character has access to, and if they're on cooldown.
/// </summary>
public class PlayerActions : EntityComponent
{
    [SerializeField] List<Move> moves = new();
    [SerializeField] GenericDictionary<Move, int> movesOnCooldown = new();
    public event Action<Move, int> CoolDownUpdated;

    public List<Move> AllMoves { get => moves; }

    protected override void Start()
    {
        base.Start();
        GameFlow.NewPlayerPhase += TickCooldown;
    }

    private void OnDestroy()
    {
        GameFlow.NewPlayerPhase -= TickCooldown;
    }

    public void TickCooldown()
    {
        foreach (var mC in movesOnCooldown.Keys)
        {
            int newValue = Mathf.Max(0, movesOnCooldown[mC] - 1);
            movesOnCooldown[mC] = newValue;
            CoolDownUpdated?.Invoke(mC, newValue);
        }
    }

    public void UsedMove(Move move)
    {
        if (!movesOnCooldown.TryAdd(move, move.Cooldown))
        {
            movesOnCooldown[move] = move.Cooldown;
        }
        CoolDownUpdated?.Invoke(move, move.Cooldown);
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
