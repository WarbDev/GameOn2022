using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Only outputs one location to the Move;
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
        targeter.Selected -= SendLocation;
        targeter.Selected += SendLocation;
        List<Location> availableRange = LocationUtility.RemoveOffMapLocations(rangeShape(playerLocation, range));
        targeter.StartTargeting(availableRange);

        KeyManager.Instance.OnEscapeDown -= Cancel;
        KeyManager.Instance.OnEscapeDown += Cancel;

        highlighter = HighlightEffectArea.Instance;
        highlighter.StartHighlighting(availableRange, effectShape, radius);
    }

    private void SendLocation(Location location)
    {
        highlighter.stopHighlighting();

        targeter.Selected -= SendLocation;

        if (location.Y == -100)
        {
            DeterminedLocations?.Invoke(null);
        }
        else
        {
            List<Location> locations = new List<Location>();
            locations.Add(location);
            DeterminedLocations?.Invoke(locations);
        }
    }

    private void Cancel()
    {
        KeyManager.Instance.OnEscapeDown -= Cancel;

        highlighter.stopHighlighting();
        targeter.Selected -= SendLocation;

        DeterminedLocations?.Invoke(null);
    }
}
