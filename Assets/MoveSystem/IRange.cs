using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Anything that gives a location output based on user input
//and is limited in range on the map
public interface IRange
{

}

public interface IRangeFromLocations: IRange
{
    public Location GetLocation(List<Location> locations);
}