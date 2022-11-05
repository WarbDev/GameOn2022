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
    [SerializeField] ClickListener tileClicker;
    [SerializeField] NewBehaviourScript move;

    public Animator animator;
    private bool isAction = false;
    private void Start()
    {
        tileClicker.EntityClicked += onClick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isAction = true;
            animator.SetBool("isBall", true);
        }
        /*
        else if(Input.GetKeyUp(KeyCode.J))
        {
            animator.SetBool("isBall", false);
        }*/
    }

    public void onClick(ClickableEntity ce)
    {
        if (isAction)
        {
            Location loc = ce.GetComponent<MapTile>().Location;
            GameObject proj = Instantiate(move.projectile);
            BallScript script = proj.GetComponent<BallScript>();
            script.endLocation = ce.GetComponent<Transform>().position;
            proj.GetComponent<Transform>().position = GetComponent<Transform>().position;
            proj.SetActive(true); //Activate the GameObject
        }
    }
}
