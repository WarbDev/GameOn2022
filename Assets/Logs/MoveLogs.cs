using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLog : ILogEntityMovement
{
    GameEntity entity;
    Location start;
    Location end;

    public GameEntity Entity { get => entity; }
    public Location Start { get => start; }
    public Location End { get => end; }

    public MoveLog(GameEntity entity, Location start, Location end)
    {
        this.entity = entity;
        this.start = start;
        this.end = end;
    }
}

public interface ILogEntityMovement
{
    public GameEntity Entity { get; }
    public Location Start { get; }
    public Location End { get; }
}