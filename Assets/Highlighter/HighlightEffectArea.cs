using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighlightEffectArea : MonoBehaviour, IHighlighter
{

    IClickChecker tileClickerListener;
    [SerializeField] Highlighter highlighter;
    private List<Location> availableRange;
    private ShapeWithRadius effectShape;
    private int radius;

    public static HighlightEffectArea Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartHighlighting(List<Location> locations, ShapeWithRadius shape, int radius)
    {
        availableRange = locations;
        effectShape = shape;
        this.radius = radius;
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver += HighlightTiles;
    }

    public void stopHighlighting()
    {
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver -= HighlightTiles;
        highlighter.StopHighlighting();
    }

    public void HighlightTiles(ClickableEntity entity)
    {
        Location location = entity.GetComponent<GameEntity>().Location;
        if (availableRange.Contains(location))
        {
            var locationsToHighlight = effectShape(location, radius).ToHashSet();
            highlighter.HighlightTiles(locationsToHighlight);
        }
        else
        {
            highlighter.StopHighlighting();
        }
    }
}