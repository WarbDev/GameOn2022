using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetBatches : MonoBehaviour
{
    public static event Action CallBatchesReset;

    
    void Start()
    {
        CallBatchesReset?.Invoke();
    }
}
