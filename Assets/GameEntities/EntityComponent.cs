using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameEntity))]
public abstract class EntityComponent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Use GetComponent to assign GameEntity on start method?")]
    bool autoAssignGameEntity = true;

    protected GameEntity gameEntity;
    public GameEntity GameEntity { get => gameEntity; }

    protected virtual void Start()
    {
        if (autoAssignGameEntity)
            gameEntity = GetComponent<GameEntity>();
    }
}