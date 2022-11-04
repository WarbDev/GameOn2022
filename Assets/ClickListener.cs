using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListener : MonoBehaviour
{
    [SerializeField] public List<ClickableEntity> clickables = new();

    private void OnEnable()
    {
        foreach(var c in clickables)
        {
            c.EntityClicked += LogClick;
        }
    }

    private void OnDisable()
    {
        foreach (var c in clickables)
        {
            c.EntityClicked -= LogClick;
        }
    }

    void LogClick(ClickableEntity ce)
    {
        Debug.Log("CLICKED! " + ce.gameObject.name);
    }
}
