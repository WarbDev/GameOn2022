using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public int health;
    private int currentHealth;

    public event Action<GameObject> IDie;

    public Move QMove;
    public Move EMove;

    void Die() {
        //oopsies
        Vector2 here = GetComponent<Transform>().position;
        //EnemyController.enemies.Remove(gameObject);
        Grid.Map[(int)here.x, (int)here.y] = null;
        IDie?.Invoke(gameObject);
        Destroy(gameObject);
    }

    public void Damage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, (float)currentHealth / (float)health, (float)currentHealth / (float)health);
    }

    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
