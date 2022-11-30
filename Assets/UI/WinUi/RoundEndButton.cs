using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoundEndButton : MonoBehaviour
{

    public event Action Click;
    
    public void onCLICKClick()
    {
        Click?.Invoke();
    }
}
