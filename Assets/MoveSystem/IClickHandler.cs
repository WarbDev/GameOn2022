using System.Collections;
using System.Collections.Generic;
using System;

public interface ITarget
{
    public event Action<Location> Selected;

    //Takes a list of locations and a highlighter shape;
    public void StartTargeting(List<Location> locations);
}