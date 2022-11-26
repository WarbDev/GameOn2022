using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnAction : MonoBehaviour
{
    public event Action<bool> DidAction;
}

public enum ActOption
{
    A,
    B,
    C,
    D
}