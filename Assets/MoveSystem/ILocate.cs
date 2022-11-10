using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Gives a list of locations 
public interface ILocate
{
    public event Action<List<Location>> DeterminedLocations;
    public void StartLocate(Move move);
}