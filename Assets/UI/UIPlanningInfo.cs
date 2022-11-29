using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIPlayerEvents : MonoBehaviour
{
    [SerializeField] Player player;

    /// <summary>
    /// Called whenever the player's health is chanegd.
    /// </summary>
    public event Action<int> HealthChanged;

    public float MaxHealth { get => player.Health.MaxHealth; }

    /// <summary>
    /// Called whenever the player starts or stops being an available option to plan movement.
    /// </summary>
    public event Action<bool> MovementNeeded;

    /// <summary>
    /// Called whenever the player starts or stops being an available option to plan an action.
    /// </summary>
    public event Action<bool> ActionNeeded;

    /// <summary>
    /// Called whenever a cooldown has been modified.
    /// </summary>
    public event Action<Move, int> CoolDownChanged;

    /// <summary>
    /// Called once a Move has finished planning, returning whether or not it was successful.
    /// </summary>
    public event Action<Move, bool> PlannedActionSuccessful;

    /// <summary>
    /// Called once a movement has finished planning, returning whether or not it was successful.
    /// </summary>
    public event Action<bool> PlannedMovementSuccessful;

    /// <summary>
    /// Called whenever the player has started planning their own movement.
    /// </summary>
    public event Action StartedPlanningMovement;

    /// <summary>
    /// Called whenever the player has started planning a move.
    /// </summary>
    public event Action<Move> StartedPlanningAction;

    private void Start()
    {
        player.Health.HealthChanged += ((health)=>HealthChanged?.Invoke(health));
        player.GetComponent<PlayerActions>().CoolDownUpdated += (Move m, int c) => CoolDownChanged?.Invoke(m, c);
        player.TurnComponent.PlannedMove += (Move m, bool s) => PlannedActionSuccessful?.Invoke(m, s);
        player.TurnComponent.PlannedMovement += ((x) => PlannedMovementSuccessful?.Invoke(x));
        player.TurnComponent.StartedPlanningMove += (x) => StartedPlanningAction?.Invoke(x);
        player.TurnComponent.StartedPlanningMovement += () => StartedPlanningMovement?.Invoke();


        StartCoroutine(TrackMovementNeeded());
        StartCoroutine(TrackActionNeeded());
    }
    IEnumerator TrackMovementNeeded()
    {
        while (true)
        {
            bool previous = player.TurnComponent.CanDoMovement();
            while (previous == player.TurnComponent.CanDoMovement())
            {
                yield return new WaitForSeconds(0.1f);
            }
            previous = player.TurnComponent.CanDoMovement();
            MovementNeeded?.Invoke(previous);
        }
    }

    IEnumerator TrackActionNeeded()
    {
        while (true)
        {
            bool previous = player.TurnComponent.CanDoAction();
            while (previous == player.TurnComponent.CanDoAction())
            {
                yield return new WaitForSeconds(0.1f);
            }
            previous = player.TurnComponent.CanDoAction();
            ActionNeeded?.Invoke(previous);
        }
    }


}
