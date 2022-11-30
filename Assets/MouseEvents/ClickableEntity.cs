using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class ClickableEntity : MonoBehaviour
{
    [SerializeField] GameEntity gameEntity;
    public Location Location { get => gameEntity.Location; }
    public event Action<ClickableEntity> EntityClicked;
    public event Action<ClickableEntity> EntityMousedOver;
    public event Action<ClickableEntity> EntityRightClicked;
    public event Action<ClickableEntity> EntityMousedOff;

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(1))
            EntityRightClicked?.Invoke(this);

        else
        EntityClicked?.Invoke(this);
    }

    private void OnMouseOver()
    {
        EntityMousedOver?.Invoke(this);
    }

    private void OnMouseExit()
    {
        EntityMousedOff?.Invoke(this);
    }

    public void DestroyEntity()
    {
        throw new NotImplementedException();
    }
}
