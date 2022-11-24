using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasSingleton : MonoBehaviour
{
    public static GameObject Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = gameObject;
    }

}
