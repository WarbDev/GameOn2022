using System.Collections;
using System.Collections.Generic;
using System;

public interface ICanDie
{
    public event Action<GameEntity> HasDied;
}
