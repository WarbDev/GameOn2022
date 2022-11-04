using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class ClickableUpdater : MonoBehaviour
{
    [SerializeField] protected ClickListener clickListener;

    protected void AddClickable(ClickableEntity entity)
    {
        clickListener.AddClickable(entity);
    }

    protected void RemoveClickable(ClickableEntity entity)
    {
        clickListener.RemoveClickable(entity);
    }
}
