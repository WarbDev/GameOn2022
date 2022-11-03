using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball: Move
{
    private int damag = 3;

    override public int damage { get => damag; set => damag = value;}

    public override void SetArea(){
        area = new Vector2Int[9];
        area[0] = new Vector2Int(4,0);
        area[1] = new Vector2Int(3,0);
        area[2] = new Vector2Int(3, -1);
        area[3] = new Vector2Int(4, -1);
        area[4] = new Vector2Int(5, -1);
        area[5] = new Vector2Int(5, 0);
        area[6] = new Vector2Int(5, 1);
        area[7] = new Vector2Int(4, 1);
        area[8] = new Vector2Int(3, 1);
    }
}
