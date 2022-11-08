using System.Collections;
using System.Collections.Generic;
using System;

public interface IClickSingle
{
    public event Action<Location> Clicked;
}
