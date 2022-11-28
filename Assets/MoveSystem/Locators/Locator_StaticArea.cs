using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        targeter.StartTargeting(area.Concat(LocationUtility.FlipLocations(area)).ToList());

        CancelScript.Instance.OnCancel -= Cancel;
        CancelScript.Instance.OnCancel += Cancel;

        highlighter = HighlightStaticArea.Instance;
        highlighter.StartHighlighting(area);
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
        CancelScript.Instance.OnCancel -= Cancel;

        highlighter.stopHighlighting();
        targeter.Selected -= SendLocation;

        DeterminedLocations?.Invoke(null);
    }
}
