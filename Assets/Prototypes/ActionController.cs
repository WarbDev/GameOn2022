using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape
{
    Square,
    Otherthings
}

public class ActionController: MonoBehaviour
{
    [SerializeField] ClickListener tileClicker;
    [SerializeField] NewBehaviourScript move;
    [SerializeField] Sprite fireTileSprite;

    public Animator animator;
    private bool isAction = false;
    private List<Location> area;
    private Location explodePoint;

    private void Awake()
    {
        this.enabled = true;
        GetComponent<Animator>().enabled = true;
    }

    private void Start()
    {
        if (transform.position.x < -1)
        {
            Destroy(gameObject);
            return;
        }
        tileClicker.EntityClicked += onClick;
        tileClicker.EntityMousedOver += onMouseOver;
        tileClicker.EntityMousedOff += onMouseOff;
       

        Location loc = new Location(0, 3);
        GetComponent<Player>().Location = loc;
        Entities.PlayerCollection.AddEntity(GetComponent<Player>()); //player needs a location, don't move this line

        area = LocationUtility.RemoveOffMapLocations(
            LocationUtility.LocationsInSquareRadius(loc, move.rangeRadius));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isAction)
            {
                isAction = false;
                animator.SetBool("isBall", false);
            }
            else
            {
                isAction = true;
                animator.SetBool("isBall", true);
            }
            
        }
    }

    public void onClick(ClickableEntity ce)
    {
        if (isAction)
        {
            MapTile tile = ce.GetComponent<MapTile>();
            if (area.Contains(tile.Location))
            {
                Location loc = tile.Location;
                GameObject proj = Instantiate(move.projectile);
                BallScript script = proj.GetComponent<BallScript>();
                script.endLocation = ce.GetComponent<Transform>().position;
                proj.GetComponent<Transform>().position = GetComponent<Transform>().position;
                proj.SetActive(true); //Activate the GameObject

                explodePoint = loc;
                script.explode += onExplode;
            }
        }
    }

    public void onExplode()
    {
        List<Location> explodeArea = LocationUtility.RemoveOffMapLocations(
            LocationUtility.LocationsInSquareRadius(explodePoint, move.fireRadius));

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(explodeArea);
        List<MapTile> tiles = LocationUtility.GetTilesInPositions(explodeArea);

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];
            Entities.EnemyCollection.RemoveEntity(enemy);
            enemy.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(enemy.gameObject);
            /*
             * damage(enemy, move.damage)
             * 
             * if(rand.nextNumber(2) = 1)
             *  damage(enemy)
             */
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            MapTile tile = tiles[i];
            tile.GetComponent<SpriteRenderer>().sprite = fireTileSprite;
        }
    }

    public void onMouseOver(ClickableEntity ce)
    {
        if (isAction)
        {
            MapTile tile = ce.GetComponent<MapTile>();
            if (area.Contains(tile.Location))
            {
                tile.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    public void onMouseOff(ClickableEntity ce)
    {
        if (isAction)
        {
            MapTile tile = ce.GetComponent<MapTile>();
            if (area.Contains(tile.Location))
            {
                tile.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
