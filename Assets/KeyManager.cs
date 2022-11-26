using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyManager : MonoBehaviour
{

    public static KeyManager Instance;

    public event Action OnEscapeDown;


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeDown?.Invoke();
        }
    }
}
