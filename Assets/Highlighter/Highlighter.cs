using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEditor;

public class Highlighter : MonoBehaviour
{
    [SerializeField] GameObject highlightPrefab;
    ObjectPool<GameObject> pool;
    Vector2 position;
    Dictionary<Location, GameObject> highlightedTiles = new();

    [SerializeField] Color color;
    public Color Color { get => color; }


    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<GameObject>(() => { return Instantiate(highlightPrefab); },
            highlight => { if (highlight) { highlight.SetActive(true); highlight.transform.position = position; } },
            highlight => { if (highlight) highlight.SetActive(false); },
            highlight => { if (highlight) Destroy(highlight); },
            true, 30, 150);
    }

    public void ChangeColor(Color newColor)
    {
        foreach (var kvp in highlightedTiles)
        {
            kvp.Value.GetComponent<Highlight>().Color = newColor;
        }
    }

    // Highlights given locations until StopHighlighting is called,
    // or if HighlighTiles is called again.
    public void HighlightTiles(HashSet<Location> locationsToHighlight)
    {
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

    public void StopHighlighting()
    {
        foreach(var key in highlightedTiles)
        {
            Location location = key.Key;
            pool.Release(highlightedTiles[location]);
            
        }
        highlightedTiles.Clear();
    }

}

