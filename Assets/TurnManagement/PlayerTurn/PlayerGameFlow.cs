using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//Keeps track of which players have completed their turns
//and store that information.
//Announces when no more players are left to complete their turns.
public static class PlayerGameFlow
{
    private static HashSet<Player> players; //The Players that need to make their turn still

    public static Action AllComplete;  //This event is called when all players are finished, or unable to finish, their turn

    //Starts up PlayerGameFLow
    //The List of players are the players that need to make their turn.
    public static void StartPlayerTurns(HashSet<Player> playersList)
    {
        players = playersList;

        foreach (Player player in players)
        {
            player.HasDied -= PlayerDied;
            player.HasDied += PlayerDied;
        }
    }


    //This method is called when a Player finishes their turn
    //It updates the Player HashSet
    public static void PlayerTurnComplete(Player player)
    {   
        CheckIfComplete(player);
    }

    private static void PlayerDied(GameEntity entity)
    {
        Player player = (Player)entity;
        CheckIfComplete(player);
    }

    private static void CheckIfComplete(Player player)
    {

        player.HasDied -= PlayerDied;
        players.Remove(player);
        if (players.Count == 0)
        {
            AllComplete?.Invoke();
        }
    }

}
