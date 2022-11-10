using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAMove : MonoBehaviour
{

    [SerializeField] Move move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 0)
        {
            Destroy(this);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            move.DoMove(this.GetComponent<Player>());
        }
    }

    
}
