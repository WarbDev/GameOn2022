using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] Health health;
    [SerializeField] SpriteRenderer healthSprite;
    [SerializeField] SpriteSet spriteSet;
    bool showHealth = false;

    private void Start()
    {
        GlobalClickListener.Instance.MapTileListener.EntityMousedOver += OnSelected;
        GlobalClickListener.Instance.MapTileListener.EntityMousedOff += OnDeselected;
    }

    private void OnDestroy()
    {
        GlobalClickListener.Instance.MapTileListener.EntityMousedOver -= OnSelected;
        GlobalClickListener.Instance.MapTileListener.EntityMousedOff -= OnDeselected;
    }

    private void Update()
    {
        healthSprite.enabled = showHealth && health.CurrentHealth > 0;
        if (healthSprite.enabled)
        {
            healthSprite.sprite = spriteSet.Sprites[(int)health.CurrentHealth - 1];
        }
        
    }

    private void OnSelected(ClickableEntity click)
    {
        if (click.Location == enemy.Location)
        showHealth = true;
    }

    void OnDeselected(ClickableEntity click)
    {
        if (click.Location == enemy.Location)
            showHealth = false;
    }
}
