using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable : IGameEntity
{
    public bool IsObstructedAt(Location location);
}

public class PushLog
{
    MoveLog moveLog;
}

public class PushRequest
{
    IPushable target;
    Location pushDirection;
    int pushForce;

    public IPushable Target { get => target; }
    public Location PushDirection { get => pushDirection; }
    public int PushForce { get => pushForce; }
}

public class Push
{

    // Push Entities
    // List<PushRequest> --> List<PushLog>
    // probably recursive/pseudo recursive
    // needs a way of tracking which entities have already been checked
    // when checking, see if push target location is occupied
    // if it is occupied by a push target, calculate that push *first*

    // what is a push?
    // 1. move in direction
    // 2. if anything is blocking, move as close as possible to that spot.

    // what does it mean to be blocking?
    // for something that would be in the way after all calculations have been made.
    // OR something that is in the way that would not be moved anywhere.

    // Things to keep track of:
    // 1. Which locations need to be calculated for pushing.

    // should not update anything on the map
    // should not play any animations
    // should return a list of movelog
    // refactor this pls
    public List<PushLog> CalculatePushes(List<PushRequest> pushRequests)
    {
        var locationsBeingPushed = GetLocationsBeingPushed(pushRequests);
        Dictionary<Location, Location> calculatedPushes = new();

        foreach (var request in pushRequests)
        {
            // get next position with obstruction
            for (int i = 1; i <= request.PushForce; i++)
            {
                IPushable target = request.Target;
                Location currentLocation = target.Location + request.PushDirection * (i - 1);
                Location nextLocation = target.Location + request.PushDirection * i;
                bool isObstructed = target.IsObstructedAt(nextLocation);
                
                // if there is an obstruction:
                if (isObstructed)
                {
                    // if obstruction's position is among the locations being pushed:
                    if (locationsBeingPushed.Contains(nextLocation))
                    {
                        // if obstruction's position has been calculated
                        if (calculatedPushes.ContainsKey(nextLocation))
                        {
                            if (calculatedPushes[nextLocation].Equals(nextLocation))
                            {
                                
                                break;
                            }

                        }
                    }

                }
                     
                
                // if there is no obstruction:
                    // add new PushLog with logged movement
                
            }
                
        }


        return null;
    }

    HashSet<Location> GetLocationsBeingPushed(IEnumerable<PushRequest> pushRequests)
    {
        HashSet<Location> locationsBeingPushed = new();
        foreach (var pushRequest in pushRequests)
        {
            locationsBeingPushed.Add(new Location(pushRequest.Target.Location.X, pushRequest.Target.Location.Y));
        }
        return locationsBeingPushed;
    }


    public void MoveTowards()
    {

    }

}