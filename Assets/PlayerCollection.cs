using System.Collections.Generic;
using System;

public static class PlayerCollection
{
    private static HashSet<Player> Players = new();

    public static event Action<Player> PlayerAdded;
    public static event Action<Player> PlayerRemoved;

    public static void AddPlayer(Player player)
    {
        Players.Add(player);
        PlayerAdded?.Invoke(player);
    }

    public static void RemovePlayer(Player player)
    {
        Players.Remove(player);
        PlayerRemoved?.Invoke(player);
    }
}
