using System.Collections;
using System;
using UnityEngine;

public class EnemyClickChecker : MonoBehaviour, IClickChecker
{
    [SerializeField] ClickListener clickListener;

    public event Action<ClickableEntity> EntityClicked;
    public event Action<ClickableEntity> EntityRightClicked;
    public event Action<ClickableEntity> EntityMousedOver;
    public event Action<ClickableEntity> EntityMousedOff;

    private void Start()
    {
        clickListener.EntityClicked += OnClicked;
        clickListener.EntityRightClicked += OnRightClicked;
        clickListener.EntityMousedOver += OnMousedOver;
        clickListener.EntityMousedOff += OnMousedOff;
    }

    void OnClicked(ClickableEntity entity)
    {
        if (entity.GetComponent<GameEntity>().EntityType == EntityType.ENEMY)
            EntityClicked?.Invoke(entity);
    }

    void OnRightClicked(ClickableEntity entity)
    {
        if (entity.GetComponent<GameEntity>().EntityType == EntityType.ENEMY)
            EntityRightClicked?.Invoke(entity);
    }

    void OnMousedOver(ClickableEntity entity)
    {
        if (entity.GetComponent<GameEntity>().EntityType == EntityType.ENEMY)
            EntityMousedOver?.Invoke(entity);
    }

    void OnMousedOff(ClickableEntity entity)
    {
        if (entity.GetComponent<GameEntity>().EntityType == EntityType.ENEMY)
            EntityMousedOff?.Invoke(entity);
    }
}
