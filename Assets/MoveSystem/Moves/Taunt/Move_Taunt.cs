using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Taunt")]

public class Move_Taunt : Move
{

    private ILocate locator;
    [SerializeField] int range;
    [SerializeField] int force;
    [SerializeField] GameObject AnimatorObject;
    public int Range { get => range; }
    ShapeWithRadiusAndDirection rangeShape = LocationUtility.LocationsInLine;
    Player player;


    public override void DoMove(Player player)
    {
        this.player = player;
        List<Location> area = rangeShape(player.Location + Directions.E, range, Directions.E);
        locator = new Locator_StaticArea(area);
        locator.DeterminedLocations -= DoEffects;
        locator.DeterminedLocations += DoEffects;
        locator.StartLocate(this);
    }

    private void DoEffects(List<Location> locations)
    {
        locator.DeterminedLocations -= DoEffects;

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        foreach (Enemy enemy in enemies)
        {
           // log.Add(enemy.Push(Directions.W, force));
        }

        PlayGraphics(new());

    }


    private void PlayGraphics(Location location)
    {
        GameObject animator = Instantiate(AnimatorObject);
        animator.transform.position = player.transform.position;
        A_Taunt animation = animator.GetComponent<A_Taunt>();

        MapTile endPoint;
        LocationUtility.TryGetTile(location, out endPoint);

        //animation.PlayAnimation(endPoint.transform.position);
    }

}
