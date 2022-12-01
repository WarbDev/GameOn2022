using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] List<Player> players = new();

    public void SpawnPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i], new Location(0, i + 1));
            players[i].Health.Heal(1);
            players[i].Animatable.SetIdleAfterAnimationEnds(true);
            players[i].Animatable.PlayAnimation<SpriteAnimationProperties>(ANIMATION_ID.ENTITY_IDLE, new(players[i].adjustedSpriteRenderer));
            players[i].ResetCooldowns();
        }
    }

    public void DespawnPlayers()
    {
        foreach(var player in players)
        {
            Entities.PlayerCollection.RemoveEntity(player);
            player.transform.position = new Vector3(200f, 200f, 200f);
        }
    }

    void SpawnPlayer(Player player, Location location)
    {
        player.SetLocation(location, true);

        Entities.PlayerCollection.AddEntity(player);
    }
}
