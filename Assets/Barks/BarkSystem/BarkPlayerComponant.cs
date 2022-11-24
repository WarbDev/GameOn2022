using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkPlayerComponant : MonoBehaviour
{
    [SerializeField] Bark talkBark;

    public int CurrentDuration = 0;
    public int MaxDuration = 1000;


    // Update is called once per frame
    void Update()
    { 
        CurrentDuration += 1;
        if (CurrentDuration >= MaxDuration)
        {
            CurrentDuration = 0;
            talkBark.PlayBark(gameObject);
        }

    }
}
