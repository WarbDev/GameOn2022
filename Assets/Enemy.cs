using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;


    void Die() {
        //oopsies
        //GetComponent<SpriteRenderer>().enabled = false;
        Vector2 here = GetComponent<Transform>().position;
        EnemyController.enemies.Remove(gameObject);
        Grid.Map[(int)here.x, (int)here.y] = null;
        Destroy(gameObject);
    }

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
        Debug.Log("I took " + damage + " damage");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Die();
        }
    }
}
