using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class HurtAnimationProperties : IAnimationProperties
{
    DamageLog damageLog;

    public HurtAnimationProperties(DamageLog damageLog)
    {
        this.damageLog = damageLog;
    }
}