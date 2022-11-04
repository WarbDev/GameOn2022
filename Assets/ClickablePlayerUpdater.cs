using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickablePlayerUpdater : MonoBehaviour, IObservePlayers
{
    [SerializeField] ClickListener clickListener;
    void Awake()
    {
        PlayerCollection.PlayerAdded += OnPlayerAdded;
        PlayerCollection.PlayerRemoved += OnPlayerRemoved;
    }

    public void OnPlayerAdded(Player player)
    {
        clickListener.AddClickable(player.GetComponent<ClickableEntity>());
    }

    public void OnPlayerRemoved(Player player)
    {
        clickListener.RemoveClickable(player.GetComponent<ClickableEntity>());
    }
}
