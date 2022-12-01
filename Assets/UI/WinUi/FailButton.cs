using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FailButton : MonoBehaviour
{
    [SerializeField] WaveWin win;

    // Start is called before the first frame update
    public void OnClick()
    {
        win.Restart();
        win.onClick();
    }
}
