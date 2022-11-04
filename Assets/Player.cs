using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGameEntity
{
    Location location;
    public Location Location { get => location; set => location = value; }

    public string Name;
}
