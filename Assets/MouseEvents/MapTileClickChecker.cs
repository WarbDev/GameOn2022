using System;
using UnityEngine;

public class MapTileClickChecker : MonoBehaviour, IClickChecker
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
        if (entity.GetComponent<IGameEntity>().EntityType == EntityType.MAPTILE)
            EntityClicked?.Invoke(entity);
    }

    void OnRightClicked(ClickableEntity entity)
    {
        if (entity.GetComponent<IGameEntity>().EntityType == EntityType.MAPTILE)
            EntityRightClicked?.Invoke(entity);
    }

    void OnMousedOver(ClickableEntity entity)
    {
        if (entity.GetComponent<IGameEntity>().EntityType == EntityType.MAPTILE)
            EntityMousedOver?.Invoke(entity);
    }

    void OnMousedOff(ClickableEntity entity)
    {
        if (entity.GetComponent<IGameEntity>().EntityType == EntityType.MAPTILE)
            EntityMousedOff?.Invoke(entity);
    }
}
