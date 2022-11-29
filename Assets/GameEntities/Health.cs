using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] bool reachedZeroHealth;

    public float CurrentHealth { get => currentHealth; }
    public float MaxHealth { get => maxHealth; }

    public event Action<GameEntity> LostAllHealth;
    public event Action<int> HealthChanged;

    public float ReduceHealth(float reduction)
    {
        if (!reachedZeroHealth)
        {
            currentHealth -= reduction;

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                reachedZeroHealth = true;
                LostAllHealth?.Invoke(gameObject.GetComponent<GameEntity>());
                
            }
            HealthChanged?.Invoke((int)currentHealth);
        }
        
        return currentHealth;
    }


}
