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



    public void AddClickable(ClickableEntity clickable)
    {
        clickables.Add(clickable);
        clickable.EntityClicked += OnClicked;
        clickable.EntityRightClicked += OnRightClicked;
        clickable.EntityMousedOver += OnMousedOver;
    }

    public void RemoveClickable(ClickableEntity clickable)
    {
        clickables.Remove(clickable);
        clickable.EntityClicked -= OnClicked;
        clickable.EntityRightClicked -= OnRightClicked;
        clickable.EntityMousedOver -= OnMousedOver;
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

}
