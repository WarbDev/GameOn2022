using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILocate
{
    public event Action<List<Location>> determinedLocations;
    public List<Location> StartLocate(IMove move);

}
