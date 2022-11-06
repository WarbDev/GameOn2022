using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGameEntity, ICanMakeMoveRequests
{
    Location location;

    public Location Location { get => location; set => location = value;}

    public MoveRequest MakeMoveRequest()
    {
        throw new System.NotImplementedException();
    }
}
