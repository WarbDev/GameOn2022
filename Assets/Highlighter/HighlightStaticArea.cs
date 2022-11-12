using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighlightStaticArea : MonoBehaviour, IHighlighter
{

    IClickChecker tileClickerListener;
    [SerializeField] Highlighter highlighter;
    private List<Location> availableArea;


    public static HighlightStaticArea Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartHighlighting(List<Location> area)
    {
        availableArea = area;
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
        Location location = entity.GetComponent<IGameEntity>().Location;
        if (availableArea.Contains(location))
        {
            var locationsToHighlight = availableArea.ToHashSet();
            highlighter.HighlightTiles(locationsToHighlight);
        }
        else
        {
            highlighter.StopHighlighting();
        }
    }
}
