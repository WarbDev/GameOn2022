using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int health;

    public Move QMove;
    public Move EMove;

    void Die() {
        //oopsies
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
