using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MovementPlanningInput : MonoBehaviour
{
    public event Action<Location> SelectedLocation;
    public event Action PlanningCancelled;

    [SerializeField] Highlighter freeHighlighter;
    [SerializeField] Highlighter unavailableHighlighter;

    List<Location> availableLocations = new();
    List<Location> unavailableLocations = new();

    public void StartRetrieving(Dictionary<Location, MovementOption> options)
    {
        availableLocations = new();
        unavailableLocations = new();
        foreach (var kvp in options)
        {
            if (kvp.Value == MovementOption.OBSTRUCTED)
            {
                unavailableLocations.Add(kvp.Key);
            }
            else
            {
                availableLocations.Add(kvp.Key);
            }
        }

        freeHighlighter.ChangeColor(Color.green);
        unavailableHighlighter.ChangeColor(Color.red);


        freeHighlighter.HighlightTiles(availableLocations.ToHashSet());
        unavailableHighlighter.HighlightTiles(unavailableLocations.ToHashSet());

        GlobalClickListener.Instance.MapTileListener.EntityClicked += LocationClicked;
        GlobalClickListener.Instance.PlayerListener.EntityClicked += LocationClicked;
    }

    void LocationClicked(ClickableEntity entity)
    {
        var location = entity.gameObject.GetComponent<GameEntity>().Location;
        if (location.X == 0)
        {
            SelectedLocation?.Invoke(location);
        }
        else
        {
            PlanningCancelled?.Invoke();
        }
    }

    public void StopRetrieving()
    {
        GlobalClickListener.Instance.MapTileListener.EntityClicked -= LocationClicked;
        GlobalClickListener.Instance.PlayerListener.EntityClicked -= LocationClicked;

        freeHighlighter.StopHighlighting();
        unavailableHighlighter.StopHighlighting();
    }

    private void OnDisable()
    {
        GlobalClickListener.Instance.MapTileListener.EntityClicked -= LocationClicked;
        GlobalClickListener.Instance.PlayerListener.EntityClicked -= LocationClicked;
    }
}
