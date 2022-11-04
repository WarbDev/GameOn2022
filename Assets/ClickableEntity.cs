using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class ClickableEntity : MonoBehaviour
{
    public event Action<ClickableEntity> EntityClicked;
    public event Action<ClickableEntity> EntityMousedOver;
    public event Action<ClickableEntity> EntityRightClicked;

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
}
