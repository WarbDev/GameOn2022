using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CancelScript : MonoBehaviour
{
    public static CancelScript Instance;

    public event Action OnCancel;
    private int leftClicked = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            leftClicked = 0;
        }

        if (leftClicked > 5 && Input.GetMouseButtonDown(1))
        {
            Cancel();
        }
        leftClicked++;
    }

    public void Cancel()
    {
        OnCancel?.Invoke();
    }
}
