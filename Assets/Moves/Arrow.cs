using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow: Move
{
    private int damag = 2;

    override public int damage { get => damag; set => damag = value;}

    public override void SetArea(){
        area = new Vector2Int[9];
        area[0] = new Vector2Int(1,0);
        area[1] = new Vector2Int(2,0);
        area[2] = new Vector2Int(3,0);
        area[3] = new Vector2Int(4, 0);
        area[4] = new Vector2Int(5, 0);
        area[5] = new Vector2Int(6, 0);
        area[6] = new Vector2Int(7, 0);
        area[7] = new Vector2Int(8, 0);
        area[8] = new Vector2Int(9, 0);
    }
}
