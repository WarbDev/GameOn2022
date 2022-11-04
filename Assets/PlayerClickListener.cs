using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerClickListener : MonoBehaviour, IObservePlayers
{
    [SerializeField] List<ClickableEntity> players;
    public event Action<Player> PlayerClick;

    public void OnPlayerAdded(Player player)
    {
        player.GetComponent<ClickableEntity>().EntityClicked += InvokeClickEvent;
    }

    public void OnPlayerRemoved(Player player)
    {
        player.GetComponent<ClickableEntity>().EntityClicked -= InvokeClickEvent;
    }

    void InvokeClickEvent(ClickableEntity playerClickableEntity)
    {
        PlayerClick?.Invoke(playerClickableEntity.gameObject.GetComponent<Player>());
    }
}
