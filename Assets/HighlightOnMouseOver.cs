using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class HighlightOnMouseOver : MonoBehaviour
{
    [SerializeField] GameObject highlightPrefab;
    IClickChecker tileClickerListener;
    ObjectPool<GameObject> pool;

    [SerializeField] int highlightRange;
    [SerializeField] Vector2 position;
    Dictionary<Location, GameObject> highlightedTiles = new();

    // Start is called before the first frame update
    private void Start()
    {
        
        pool = new ObjectPool<GameObject>(() => { return Instantiate(highlightPrefab); }, highlight => { highlight.SetActive(true); highlight.transform.position = position; },
            highlight => { highlight.SetActive(false); }, highlight => { Destroy(highlight); }, true, 30, 150);

        tileClickerListener = GlobalClickListener.Instance.MapTileListener;
        tileClickerListener.EntityMousedOver += HighlightTile;
    }

    void HighlightTile(ClickableEntity entity)
    {
        var locationsToHighlight = LocationUtility.LocationsInSquareRadius(entity.GetComponent<IGameEntity>().Location, highlightRange).ToHashSet();
        List<Location> locationsToRemove = new();

        foreach (var location in highlightedTiles)
        {
            if (!locationsToHighlight.Contains(location.Key))
            {
                locationsToRemove.Add(location.Key);
            }
        }

        foreach (var location in locationsToHighlight)
        {
            if (!highlightedTiles.ContainsKey(location) && LocationUtility.IsOnMap(location))
            {
                position = new Vector2(location.X, location.Y);
                highlightedTiles.Add(location, pool.Get());
            }
        }

        foreach (var location in locationsToRemove)
        {
            pool.Release(highlightedTiles[location]);
            highlightedTiles.Remove(location);
        }
        locationsToRemove.Clear();
    }
}
