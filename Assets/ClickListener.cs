using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickListener : MonoBehaviour
{
    [SerializeField] public List<ClickableEntity> clickables = new();

    public event Action<ClickableEntity> EntityClicked;
    public event Action<ClickableEntity> EntityRightClicked;
    public event Action<ClickableEntity> EntityMousedOver;
    public event Action<ClickableEntity> EntityMousedOff;



    public void AddClickable(ClickableEntity clickable)
    {
        clickables.Add(clickable);
        clickable.EntityClicked += OnClicked;
        clickable.EntityRightClicked += OnRightClicked;
        clickable.EntityMousedOver += OnMousedOver;
        clickable.EntityMousedOff += OnMousedOff;
    }

    public void RemoveClickable(ClickableEntity clickable)
    {
        clickables.Remove(clickable);
        clickable.EntityClicked -= OnClicked;
        clickable.EntityRightClicked -= OnRightClicked;
        clickable.EntityMousedOver -= OnMousedOver;
        clickable.EntityMousedOff -= OnMousedOff;
    }

    void OnClicked(ClickableEntity entity)
    {
        EntityClicked?.Invoke(entity);
    }

    void OnRightClicked(ClickableEntity entity)
    {
        EntityRightClicked?.Invoke(entity);
    }

    void OnMousedOver(ClickableEntity entity)
    {
        EntityMousedOver?.Invoke(entity);
    }

    void OnMousedOff(ClickableEntity entity)
    {
        EntityMousedOff?.Invoke(entity);
    }

}
