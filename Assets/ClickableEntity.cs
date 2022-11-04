using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class ClickableEntity : MonoBehaviour
{
    public event Action<ClickableEntity> EntityClicked; 
    private void OnMouseDown()
    {
        EntityClicked?.Invoke(this);
    }
}
