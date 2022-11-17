using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public interface IPushable : IGameEntity
{
    public bool IsObstructedAt(Location location);
}

public class PushLog
{
    public MoveLog MoveLog;
    public PushLog(MoveLog moveLog)
    {
        MoveLog = moveLog;
    }
}

public class PushRequest
{
    IPushable target;
    Location pushDirection;
    int pushForce;

    public PushRequest(IPushable target, Location pushDirection, int pushForce)
    {
        this.target = target;
        this.pushDirection = pushDirection;
        this.pushForce = pushForce;
    }

    public IPushable Target { get => target; }
    public Location PushDirection { get => pushDirection; }
    public int PushForce { get => pushForce; }

}

public class Push
{


    public static void DoPushes(List<PushLog> pushes)
    {
        List<IGameEntity> entities = new();
        List<Location> endPoints = new();
        for (int i = 0; i < pushes.Count; i++)
        {
            entities.Add(pushes[i].MoveLog.Entity);
            endPoints.Add(pushes[i].MoveLog.End);
        }
        GameMap.MoveEnemies(entities, endPoints);
    }

    // Push Entities
    // List<PushRequest> --> List<PushLog>
    // recursive
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

    public static List<PushLog> CalculatePushes(List<PushRequest> pushRequests)
    {
        if (pushRequests.Count == 0)
        {
            return new List<PushLog>();
        }

        HashSet<PushRequest> requests = pushRequests.ToHashSet();

        PushRequest subject = requests.First();
        requests.Remove(subject);

        Dictionary<IPushable, PushRequest> pushables = new();
        foreach (PushRequest request in requests)
        {
            pushables.Add(request.Target, request);
        }

        return Calc(pushables, subject, new List<PushLog>());
    }

    private static List<PushLog> Calc(Dictionary<IPushable, PushRequest> pushables, PushRequest subject, List<PushLog> resultSoFar)
    {
        IGameEntity obstacle = CheckTowards(subject, resultSoFar);
        if (obstacle is IPushable && pushables.ContainsKey((IPushable)obstacle)) //If obstacle is in the Dictionary
        {
            IPushable pushable = (IPushable) obstacle;
            PushRequest pushableRequest = pushables[pushable];
            pushables.Remove(pushable);
            Calc(pushables, pushableRequest, resultSoFar);

        }

        if (pushables.Count == 0)  //Base Case, No more values in Dictionary
        {
            PushLog pushLog = MoveForwards(subject, resultSoFar);
            resultSoFar.Add(pushLog);
            Debug.Log(string.Format("I Pushed to ({0},{1})", pushLog.MoveLog.End.X, pushLog.MoveLog.End.Y));
            return resultSoFar;
        }

        PushLog log = MoveForwards(subject, resultSoFar);
        resultSoFar.Add(log);
        PushRequest push = pushables.First().Value;
        pushables.Remove(push.Target);
        Debug.Log(string.Format("I Pushed to ({0},{1})", log.MoveLog.End.X, log.MoveLog.End.Y));
        return Calc(pushables, push, resultSoFar);
    }

    private static PushLog MoveForwards(PushRequest subject, List<PushLog> resultSoFar)
    {

        Location startLocation = subject.Target.Location;
        IGameEntity me = subject.Target;
        for (int i = 1; i <= subject.PushForce; i++)
        {
            Location pointToCheck = subject.Target.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i);
            if (!LocationUtility.IsOnMap(pointToCheck))
            {
                Location edge = subject.Target.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i-1);
                return new PushLog(new MoveLog(me, startLocation, edge));
            }

            List<IGameEntity> entities = LocationUtility.GetEntitiesAtPosition(pointToCheck);

            //Adds and removes entities from resultSoFar
            foreach (PushLog log in resultSoFar)
            {
                if (pointToCheck.Equals(log.MoveLog.End))
                {
                    entities.Add(log.MoveLog.Entity);
                }
                else if (pointToCheck.Equals(log.MoveLog.Start))
                {
                    entities.Remove(log.MoveLog.Entity);
                }
            }

            foreach (IGameEntity entity in entities)
            {
                if (subject.Target.IsObstructedBy(entity))
                {
                    Location endLocation = subject.Target.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i-1);
                    return new PushLog(new MoveLog(me, startLocation, endLocation));
                }
            }
        }
        Location end = subject.Target.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, subject.PushForce);
        return new PushLog(new MoveLog(me, startLocation, end));
    }

    //Checks all the spaces in front of the subject PushRequest
    //Ignores game entities on the map if they are in resultSoFar,
    //Instead checks their end position
    //Returns the first obstical it finds
    //Returns null if there are no obsticals
    private static IGameEntity CheckTowards(PushRequest subject, List<PushLog> resultSoFar)
    {
        IGameEntity obstructor = null;
        for (int i = 1; i <= subject.PushForce; i++)
        {
            Location pointToCheck = subject.Target.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i);
            List<IGameEntity> entities = LocationUtility.GetEntitiesAtPosition(pointToCheck);

            //Adds and removes entities from resultSoFar
            foreach (PushLog log in resultSoFar)
            {
                if (pointToCheck.Equals(log.MoveLog.End) && !pointToCheck.Equals(log.MoveLog.Start))
                {
                    entities.Add(log.MoveLog.Entity);
                }
                else if (pointToCheck.Equals(log.MoveLog.Start))
                {
                    entities.Remove(log.MoveLog.Entity);
                }
            }

            foreach (IGameEntity entity in entities)
            {
                if (subject.Target.IsObstructedBy(entity))
                {
                    return entity;
                }
            }
        }
        return obstructor;
    }

}