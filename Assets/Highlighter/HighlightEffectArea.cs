using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighlightEffectArea : MonoBehaviour, IHighlighter
{

    IClickChecker tileClickerListener;
    [SerializeField] Highlighter highlighter;
    private HashSet<Location> availableRange;
    private ShapeWithRadius effectShape;
    private int radius;

    private bool onMap;
    private bool isHighlighting;

    public static HighlightEffectArea Instance;

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
            highlighter.HighlightTiles(availableRange);
        }
    }

    public void StartHighlighting(List<Location> locations, ShapeWithRadius shape, int radius)
    {
        isHighlighting = true;
        availableRange = locations.ToHashSet();
        effectShape = shape;
        this.radius = radius;
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver += HighlightTiles;
        tileClickerListener.EntityMousedOff += MouseOff;
        highlighter.ChangeColor(new Color(1, 0.92f, 0.016f, .5f)); //Yellow
        highlighter.HighlightTiles(availableRange);
    }

    public void stopHighlighting()
    {
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver -= HighlightTiles;
        tileClickerListener.EntityMousedOff -= MouseOff;
        isHighlighting = false;
        onMap = false;
        highlighter.StopHighlighting();
    }

    private void HighlightTiles(ClickableEntity entity)
    {
        onMap = true;
        Location location = entity.GetComponent<GameEntity>().Location;
        if (availableRange.Contains(location))
        {
            var locationsToHighlight = effectShape(location, radius).ToHashSet();
            highlighter.ChangeColor(new Color(0, 1, 0, .5f)); //Green
            highlighter.HighlightTiles(locationsToHighlight);
        }
        else
        {
            highlighter.ChangeColor(new Color(1, 0.92f, 0.016f, .5f)); //Yellow
            highlighter.HighlightTiles(availableRange);
        }
    }

    private void MouseOff(ClickableEntity entity)
    {
        onMap = false;
    }
}