using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    private int currentHealth;

    void Die() {
        //oopsies
        //GetComponent<SpriteRenderer>().enabled = false;
        Vector2 here = GetComponent<Transform>().position;
        EnemyController.enemies.Remove(gameObject);
        Grid.Map[(int)here.x, (int)here.y] = null;
        Destroy(gameObject);
    }

    public void Damage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
        Debug.Log("I took " + damage + " damage");
        GetComponent<SpriteRenderer>().color = new Color(1f, (float)currentHealth / (float)health, (float)currentHealth / (float)health);
    }


    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Die();
        }
    }
}
