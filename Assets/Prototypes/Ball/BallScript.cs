using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 endLocation;
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
            GetComponent<Animator>().SetBool("atEnd", true);
        }
    }
}
