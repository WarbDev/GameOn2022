using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] bool reachedZeroHealth;

    public float CurrentHealth { get => currentHealth; private set => currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    public float MaxHealth { get => maxHealth; }

    public event Action<GameEntity> LostAllHealth;
    public event Action<int> Healed;

    public float ReduceHealth(float reduction)
    {
        if (!reachedZeroHealth)
        {
            currentHealth -= reduction;

            if (currentHealth <= 0.1f)
            {
                currentHealth = 0f;
                reachedZeroHealth = true;
                LostAllHealth?.Invoke(gameObject.GetComponent<GameEntity>());
                
            }
            
        }
        return currentHealth;
    }

    public int Heal(int amount)
    {
        CurrentHealth = CurrentHealth + amount;
        Healed?.Invoke((int)currentHealth);
        return (int)currentHealth;
    }

}
