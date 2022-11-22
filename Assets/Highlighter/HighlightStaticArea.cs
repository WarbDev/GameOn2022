using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighlightStaticArea : MonoBehaviour, IHighlighter
{

    IClickChecker tileClickerListener;
    [SerializeField] Highlighter highlighter;
    private HashSet<Location> availableArea;
    private HashSet<Location> otherArea;
    private HashSet<Location> totalArea;

    public static HighlightStaticArea Instance;

    private bool onMap = false;
    private bool isHighlighting;

    private void Awake()
    {
        Instance = this;
        onMap = false;
        isHighlighting = false;
    }

    private void Update()
    {
        if (!onMap && isHighlighting)
        {
            highlighter.ChangeColor(new Color(1, 0.92f, 0.016f, .5f)); //Yellow
            highlighter.HighlightTiles(totalArea);
        }
    }

    public void StartHighlighting(List<Location> area)
    {
        isHighlighting = true;
        availableArea = area.ToHashSet();
        otherArea = LocationUtility.FlipLocations(area).ToHashSet();
        totalArea = availableArea.Concat(otherArea).ToHashSet();
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver += HighlightTiles;
        tileClickerListener.EntityMousedOff += MouseOff;
        highlighter.ChangeColor(new Color(1, 0.92f, 0.016f, .5f)); //Yellow
        highlighter.HighlightTiles(totalArea);
    }

    public void stopHighlighting()
    {
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver -= HighlightTiles;
        isHighlighting = false;
        onMap = false;
        highlighter.StopHighlighting();
    }

    public void HighlightTiles(ClickableEntity entity)
    {
        onMap = true;
        Location location = entity.GetComponent<GameEntity>().Location;
        if (availableArea.Contains(location))
        {
            highlighter.ChangeColor(new Color(0, 1, 0, .5f)); //Green
            highlighter.HighlightTiles(availableArea);
        } 
        else if (otherArea.Contains(location))
        {
            highlighter.ChangeColor(new Color(0, 1, 0, .5f)); //Green
            highlighter.HighlightTiles(otherArea);
        }
        else
        {
            highlighter.ChangeColor(new Color(1, 0.92f, 0.016f, .5f)); //Yellow
            highlighter.HighlightTiles(totalArea);
        }
    }

    private void MouseOff(ClickableEntity entity)
    {
        onMap = false;
    }
}
