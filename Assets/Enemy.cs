using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGameEntity
{
    Location location;

    public Location Location { get => location; set => location = value;}

}
