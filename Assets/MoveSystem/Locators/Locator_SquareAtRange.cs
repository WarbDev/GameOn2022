using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Locator_SquareAtRange : ILocate
{
    public event Action<List<Location>> determinedLocations;
    private int range;
    private int radius;
    private IClickSingle clickHandler;
    public Locator_SquareAtRange(int rag, int rad)
    {
        range = rag;
        radius = rad;
    }
    public List<Location> StartLocate(IMove move)
    {
        clickHandler = new Click_OneWithinRange(range);
        clickHandler.Clicked += createArea;
        return null;
    }

    private void createArea(Location location)
    {
        clickHandler.Clicked -= createArea;
        List<Location> locations = LocationUtility.LocationsInSquareRadius(location, radius);
        determinedLocations?.Invoke(locations); //impliment
    }
}
