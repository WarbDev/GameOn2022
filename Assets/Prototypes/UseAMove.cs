using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAMove : MonoBehaviour
{

    [SerializeField] Move moveA;
    [SerializeField] Move moveB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 0)
        {
            Destroy(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            moveA.DoMove(GetComponent<Player>());
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            moveB.DoMove(GetComponent<Player>());
        }
    }

    
}
