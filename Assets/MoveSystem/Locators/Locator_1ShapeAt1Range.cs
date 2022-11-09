using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Locator_1ShapeAt1Range : ILocate
{
    public event Action<List<Location>> DeterminedLocations;
    private ShapeWithRadius rangeShape; //A Delegate
    private ShapeWithRadius effectShape;
    ITarget targeter;
    //IHighlight
    private Location playerLocation;
    private int range;
    private int radius;

    public Locator_1ShapeAt1Range(ShapeWithRadius rangeShape, ShapeWithRadius effectShape, Location loc, int range, int radius)
    {
        this.rangeShape = rangeShape;
        this.effectShape = effectShape;
        playerLocation = loc;
        this.range = range;
        this.radius = radius;
    }
    public void StartLocate(IMove move)
    {
        targeter.Selected -= CreateArea;
        targeter.Selected += CreateArea;
        List<Location> availableRange = LocationUtility.RemoveOffMapLocations(rangeShape(playerLocation, range));
        targeter = Select_OneWithinRange.Instance;

        targeter.StartTargeting(availableRange);
    }

    private void CreateArea(Location location)
    {
        targeter.Selected -= CreateArea;
        List<Location> locations = LocationUtility.LocationsInSquareRadius(location, radius);
        DeterminedLocations?.Invoke(locations);
    }
}
