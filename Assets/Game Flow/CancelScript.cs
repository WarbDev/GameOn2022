using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CancelScript : MonoBehaviour
{
    public static CancelScript Instance;

    public event Action OnCancel;


    private void Awake()
    {
        Instance = this;
    }

    public void Cancel()
    {
        OnCancel?.Invoke();
    }
}
