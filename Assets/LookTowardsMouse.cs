using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsMouse : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        GlobalClickListener.Instance.MapTileListener.EntityMousedOver += UpdateSpritePos;
    }

    void UpdateSpritePos(ClickableEntity e)
    {
        float x = Mathf.Sign(e.GetComponent<GameEntity>().Location.X);
        if (x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDisable()
    {
        
    }
}
