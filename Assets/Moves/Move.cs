using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    virtual public int damage { get; set; }
    public Vector2Int[] area;

    void Start() {
        SetArea();
    }

    public virtual void SetArea() {}

    public Vector2Int Translate(Vector2Int rel, Vector2 pos) {
        return new Vector2Int((int)pos.x + rel.x, (int)pos.y + rel.y);
    }

    protected void DealDamage(GameObject user) {
        Debug.Log("I am here the first position of area is " + area[0].x);
        Transform t = user.GetComponent<Transform>();
        for (int i = 0; i < area.Length; i++) {
            Vector2Int pos = Translate(area[i], t.position); //relative position + actual position
            if(pos.x >= 0 && pos.x < Grid.Map.GetLength(0)) {
                if (pos.y >= 0 && pos.y < Grid.Map.GetLength(1)) {
                    GameObject entity = Grid.Map[pos.x, pos.y];
                    if (entity) {
                        entity.GetComponent<Enemy>().Damage(damage);
                    }
                }
            }
        }
    }

    public void Use(GameObject user) {
        DealDamage(user);
    }
}
