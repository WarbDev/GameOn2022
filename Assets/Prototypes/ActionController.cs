using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shape
{
    Square,
    Otherthings
}

public class ActionController: MonoBehaviour
{
    [SerializeField] ClickListener clicky;



    public Animator animator;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetBool("isBall", true);
        }
        else if(Input.GetKeyUp(KeyCode.J))
        {
            animator.SetBool("isBall", false);
        }
    }
}
