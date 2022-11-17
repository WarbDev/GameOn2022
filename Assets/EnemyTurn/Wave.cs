using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [Range(1, 100)] [SerializeField] int batchSpeed;
    [Range(3, 8)]   [SerializeField] int height;
    [Range(1, 16)]  [SerializeField] int rightLength;
    [Range(0, 16)]  [SerializeField] int leftLength;

    
}
