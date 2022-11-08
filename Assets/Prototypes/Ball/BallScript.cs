using System.Collections;
using System;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 endLocation;
    public event Action explode;

    private float entityTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform trans = transform;

        trans.position = Vector2.MoveTowards(trans.position, endLocation, .05f);
        Vector2 pos = trans.position;
        if (endLocation.x - pos.x < .5f && endLocation.y - pos.y < .5f)
        {
            if (entityTime > 0)
            {
                if (Time.time - entityTime > .3)
                {
                    explode?.Invoke();
                    Destroy(gameObject);
                }
            }
            else
            {
                entityTime = Time.time;
                GetComponent<Animator>().SetBool("atEnd", true);
                Debug.Log(entityTime);
            }
        }

    }
}
