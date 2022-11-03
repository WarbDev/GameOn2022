using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash: Move
{
    private int damag = 3;

    override public int damage { get => damag; set => damag = value;}

    public override void SetArea(){
        area = new Vector2Int[3];
        area[0] = new Vector2Int(1,0);
        area[1] = new Vector2Int(1,1);
        area[2] = new Vector2Int(1, -1);
    }
}
