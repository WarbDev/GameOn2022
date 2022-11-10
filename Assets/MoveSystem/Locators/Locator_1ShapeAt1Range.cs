using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Locator_1ShapeAt1Range : ILocate
{
    public event Action<List<Location>> DeterminedLocations;
    private ShapeWithRadius rangeShape; //A Delegate
    private ShapeWithRadius effectShape;
    HighlightEffectArea highlighter;
    ITarget targeter;
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
    public void StartLocate(Move move)
    {
        targeter = new Select_OneWithinRange();
        targeter.Selected -= CreateArea;
        targeter.Selected += CreateArea;
        List<Location> availableRange = LocationUtility.RemoveOffMapLocations(rangeShape(playerLocation, range));
        targeter.StartTargeting(availableRange);

        highlighter = HighlightEffectArea.Instance;
        highlighter.StartHighlighting(availableRange, effectShape, radius);
    }

    private void CreateArea(Location location)
    {
        highlighter.stopHighlighting();

        targeter.Selected -= CreateArea;
        List<Location> locations = LocationUtility.LocationsInSquareRadius(location, radius);
        DeterminedLocations?.Invoke(locations);
    }
}
