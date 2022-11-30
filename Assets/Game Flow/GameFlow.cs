using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Container for various game flow events. UNSUBSCRIBE FROM THESE ONDESTROY.
/// </summary>
public class GameFlow : MonoBehaviour
{
    [SerializeField] PhaseEnd phaseEnd;
    [SerializeField] DeathAnimationTicker deathAnimationTicker;
    

    /// <summary>
    /// Invoked when a new player phase has started.
    /// </summary>
    public static event Action NewPlayerPhase;

    /// <summary>
    /// Invoked when a new round has started.
    /// </summary>
    public static event Action NewRound;

    /// <summary>
    /// Invoked after a move has finished or an enemy has finished a movement/action tick.
    /// </summary>
    public static event Action DeathAnimationTick;

    private void Start()
    {
        phaseEnd.NewRound += (() => NewRound?.Invoke());
        phaseEnd.NewRound += TickTimers;
        phaseEnd.NewPlayerPhase += (() => NewPlayerPhase?.Invoke());
        deathAnimationTicker.DeathAnimationTick += TickDeathAnimation;
    }

    void TickDeathAnimation()
    {
        DeathAnimationTick?.Invoke();
        Debug.Log("DEath naimat clicked");
    }

    void TickTimers()
    {
        List<ITickable> list = new List<ITickable>();

        foreach(var enemy in Entities.EnemyCollection.EntitiesSet)
        {
            var tickable = (enemy as ITickable);
            if (tickable != null) list.Add(tickable);
        }

        foreach(var terrainEffect in Entities.TerrainCollection.EntitiesSet)
        {
            var tickable = (terrainEffect as ITickable);
            if (tickable != null) list.Add(tickable);
        }

        foreach(var player in Entities.PlayerCollection.EntitiesSet)
        {
            var tickable = player as ITickable;
            if (tickable != null) list.Add(tickable);
        }

        foreach(var tickable in list)
        {
            tickable.OnRoundTick();
        }
    }
}
