using System.Collections;
using System.Collections.Generic;
using System;

public interface ITarget
{
    public event Action<Location> Selected;

    public void StartTargeting(List<Location> locations);
}