using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservePlayers
{
    public void OnPlayerAdded(Player player);
    public void OnPlayerRemoved(Player player);
}
