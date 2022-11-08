using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Click_OneWithinRange : IClickSingle
{
    public event Action<Location> Clicked;
    private IClickChecker clickListener = GlobalClickListener.Instance.MapTileListener;
    private int range;

    public Click_OneWithinRange(int range)
    {
        this.range = range;
        clickListener.EntityClicked += located;
    }

    private void located(ClickableEntity tile)
    {
        Clicked?.Invoke(tile.gameObject.GetComponent<MapTile>().Location);
    }
}
