using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator_StaticArea : ILocate
{
    public event Action<List<Location>> DeterminedLocations;
    ITarget targeter;
    HighlightStaticArea highlighter;

    List<Location> area;

    public Locator_StaticArea(List<Location> area)
    {
        this.area = LocationUtility.RemoveOffMapLocations(area);
    }

    public void StartLocate(Move move)
    {
        targeter = new Select_OneWithinRange();
        targeter.Selected -= SendLocation;
        targeter.Selected += SendLocation;
        targeter.StartTargeting(area);

        highlighter = HighlightStaticArea.Instance;
        highlighter.StartHighlighting(area);
    }

    private void SendLocation(Location location)
    {
        highlighter.stopHighlighting();

        targeter.Selected -= SendLocation;
        List<Location> locations = new List<Location>();
        locations.Add(location);
        DeterminedLocations?.Invoke(locations);
    }
}
