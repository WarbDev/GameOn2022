using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

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
        for (int i = 0; i < pushes.Count; i++)
        {
            GameEntity entity = pushes[i].MoveLog.Entity;
            Location end = pushes[i].MoveLog.End;
            if (entity is Enemy)
            {
                GameMap.MoveEnemy(entity, end);
            } 
            else if (entity is TerrainBase)
            {
                GameMap.MoveTerrain(entity, end);
            }
        }
        
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
        IObstruct obstacle = CheckTowards(subject, resultSoFar);
        IPushable pushable = null;
        if (obstacle != null)
        {
            pushable = obstacle.Entity as IPushable;
        }
        
        if (pushable != null && pushables.ContainsKey(pushable)) //If obstacle is in the Dictionary
        {
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

        Location startLocation = subject.Target.Entity.Location;
        GameEntity me = subject.Target.Entity;

        for (int i = 1; i <= subject.PushForce; i++)
        {
            Location pointToCheck = subject.Target.Entity.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i);
            if (!LocationUtility.IsOnMap(pointToCheck))
            {
                Location edge = subject.Target.Entity.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i-1);
                return new PushLog(new MoveLog(me, startLocation, edge));
            }

            List<IObstruct> obstructingEntities = LocationUtility.GetObstructionsAtPosition(pointToCheck);

            //Adds and removes entities from resultSoFar
            foreach (PushLog log in resultSoFar)
            {
                if (pointToCheck.Equals(log.MoveLog.End))
                {
                    obstructingEntities.Add(log.MoveLog.Entity as IObstruct);
                }
                else if (pointToCheck.Equals(log.MoveLog.Start))
                {
                    obstructingEntities.Remove(log.MoveLog.Entity as IObstruct);
                }
            }

            foreach (var entity in obstructingEntities)
            {
                if (subject.Target.Pushable.IsObstructedBy(entity))
                {
                    Location endLocation = subject.Target.Entity.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i-1);
                    return new PushLog(new MoveLog(me, startLocation, endLocation));
                }
            }
        }
        Location end = subject.Target.Entity.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, subject.PushForce);
        return new PushLog(new MoveLog(me, startLocation, end));
    }

    //Checks all the spaces in front of the subject PushRequest
    //Ignores game entities on the map if they are in resultSoFar,
    //Instead checks their end position
    //Returns the first obstical it finds
    //Returns null if there are no obsticals
    private static IObstruct CheckTowards(PushRequest subject, List<PushLog> resultSoFar)
    {
        IObstruct obstructor = null;
        for (int i = 1; i <= subject.PushForce; i++)
        {
            Location pointToCheck = subject.Target.Entity.Location + LocationUtility.CalculateRelativeLocationFromDirectionAndMagnitude(subject.PushDirection, i);
            List<IObstruct> obstructingEntities = LocationUtility.GetObstructionsAtPosition(pointToCheck);

            //Adds and removes entities from resultSoFar
            foreach (PushLog log in resultSoFar)
            {
                if (pointToCheck.Equals(log.MoveLog.End) && !pointToCheck.Equals(log.MoveLog.Start))
                {
                    obstructingEntities.Add(log.MoveLog.Entity as IObstruct);
                }
                else if (pointToCheck.Equals(log.MoveLog.Start))
                {
                    obstructingEntities.Remove(log.MoveLog.Entity as IObstruct);
                }
            }

            foreach (var entity in obstructingEntities)
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