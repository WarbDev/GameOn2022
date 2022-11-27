using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Component for containing a list of tags that other classes can compare against to see if an obstruction has been made.
public class Obstruction : EntityComponent, IObstruct
{
    public GameEntity Entity { get => GameEntity; }
    public IObstruct Obstruct { get => this; }
    [Tooltip("Do not have duplicate instances! Gets converted to a HashSet on Awake()." +
        " Duplicate elements won't throw an exception, but just don't do it.")]
    [SerializeField] List<OBSTRUCTION_TAG> obstructionTagList = new();

    HashSet<OBSTRUCTION_TAG> obstructionTags = new();
    public HashSet<OBSTRUCTION_TAG> ObstructionTags { get => obstructionTags; }

    private void Awake()
    {
        obstructionTags = obstructionTagList.ToHashSet();
    }
}

public interface IObstruct : IHaveGameEntity
{
    public IObstruct Obstruct { get; }
    public HashSet<OBSTRUCTION_TAG> ObstructionTags { get => Obstruct.ObstructionTags; }
}

public enum OBSTRUCTION_TAG
{
    ENEMY,
    PLAYER,
    BARRICADE,
    BOMB
}
