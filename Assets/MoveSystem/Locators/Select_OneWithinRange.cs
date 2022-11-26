using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Select_OneWithinRange: ITarget
{
    public event Action<Location> Selected;
    public static Select_OneWithinRange Instance;
    private List<Location> availableLocations;

    private IClickChecker clickListener;



    /*private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        clickListener = GlobalClickListener.Instance.MapTileListener;
    }*/


    public void StartTargeting(List<Location> locations)
    {
        clickListener = GlobalClickListener.Instance.MapTileListener;
        clickListener.EntityClicked -= Located;
        clickListener.EntityClicked += Located;
        availableLocations = locations;
    }

    private void Located(ClickableEntity tile)
    {
        Location loc = tile.gameObject.GetComponent<MapTile>().Location;
        if (availableLocations.Contains(loc))
        {
            Selected?.Invoke(loc);
            clickListener.EntityClicked -= Located;
        }
        else
        {
            Selected?.Invoke(new Location(-100,-100));
            clickListener.EntityClicked -= Located;
        }
    }
}
