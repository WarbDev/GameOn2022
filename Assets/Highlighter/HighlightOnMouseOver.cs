using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class HighlightOnMouseOver : MonoBehaviour
{
    IClickChecker tileClickerListener;
    [SerializeField] Highlighter highlighter;
    [SerializeField] int highlightRange;

    // Start is called before the first frame update
    private void OnEnable()
    {
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver += HighlightTiles;
    }

    private void OnDisable()
    {
        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver -= HighlightTiles;
        highlighter.StopHighlighting();
    }

    public void HighlightTiles(ClickableEntity entity)
    {
        var locationsToHighlight = LocationUtility.LocationsInSquareRadius(entity.GetComponent<GameEntity>().Location, highlightRange).ToHashSet();
        highlighter.HighlightTiles(locationsToHighlight);
    }
}
