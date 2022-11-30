using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEditor;

public class Highlighter : MonoBehaviour
{
    [SerializeField] Highlight highlightPrefab;
    ObjectPool<Highlight> pool;
    Vector2 position;
    Dictionary<Location, Highlight> highlightedTiles = new();

    [SerializeField] Color color;
    public Color Color { get => color; }


    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<Highlight>(() => { return Instantiate(highlightPrefab); },
            highlight => { if (highlight) { highlight.gameObject.SetActive(true); highlight.Color = color; highlight.transform.position = position; } },
            highlight => { highlight.gameObject.SetActive(false); },
            highlight => { Destroy(highlight.gameObject); },
            true, 30, 150);
    }

    public void ChangeColor(Color newColor)
    {
        color = newColor;
        foreach (var kvp in highlightedTiles)
        {
            kvp.Value.GetComponent<Highlight>().Color = color;
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
                Highlight highlight = pool.Get();
                highlight.Color = Color;
                highlightedTiles.Add(location, highlight);
                
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

