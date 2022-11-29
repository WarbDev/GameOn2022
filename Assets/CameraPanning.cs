using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    //[SerializeField] float panning;
    [SerializeField] Camera myCamera;

    [Range(1,10)]
    public int maxCameraRight;
    [Range(1, 10)]
    public int maxCameraDown;
    [Range(1, 10)]
    public int maxCameraLeft;
    [Range(1, 10)]
    public int maxCameraUp;


    private int cameraHorizontal = 0;
    private int cameraVertical = -1;


    private int speed = 1;

    
    private int horizonalTime = 0;
    private int verticalTime = 0;
    //private bool startedPressingButton = true;
    private int waitTime = 20;

    private int moveTime = 0;
    private int timePerCameraMove = 20;

    private bool isHoldingRight { get => Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D); }
    private bool isHoldingDown { get => Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S); }
    private bool isHoldingLeft { get => Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A); }
    private bool isHoldingUp { get => Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W); }

    private void Update()
    {
        bool holdingButton = false;
        if (isHoldingRight)
        {
            holdingButton = true;
            horizonalTime++;
            if(moveDirection(new Vector3(speed, 0), horizonalTime, cameraHorizontal, maxCameraRight))
            {
                cameraHorizontal++;
            }
        }
        else if (isHoldingLeft)
        {
            holdingButton = true;
            horizonalTime++;
            if(moveDirection(new Vector3(-speed, 0), horizonalTime, cameraHorizontal, -maxCameraLeft))
            {
                cameraHorizontal--;
            }
        }

        if (isHoldingDown)
        {
            holdingButton = true;
            verticalTime++;
            if(moveDirection(new Vector3(0, -speed), verticalTime, cameraVertical, -maxCameraDown))
            {
                cameraVertical--;
            }
        }
        else if (isHoldingUp)
        {
            holdingButton = true;
            verticalTime++;
            if(moveDirection(new Vector3(0, speed), verticalTime, cameraVertical, maxCameraUp))
            {
                cameraVertical++;
            }
        }

        if (!holdingButton)
        {
            horizonalTime = 0;
            verticalTime = 0;
            moveTime = 999;
        }
    }

    private bool moveDirection(Vector3 direction, int time, int bound, int maxBound)
    {

        if (time > waitTime)
        {
            moveTime++;
            if (moveTime >= timePerCameraMove)
            {
                if (maxBound * bound > 0) //same side
                {
                    if (Mathf.Abs(bound) < Mathf.Abs(maxBound))
                    {
                        if (Mathf.Abs(direction.y) > 0)
                        {
                            moveTime = 0;
                            myCamera.transform.position += direction;
                            Vector3 rotation = myCamera.transform.localEulerAngles;
                            myCamera.transform.localEulerAngles = new Vector3(rotation.x + 10 * direction.y, rotation.y, rotation.z);
                            return true;
                        }
                        else
                        {
                            moveTime = 0;
                            myCamera.transform.position += direction;
                            return true;
                        }
                    }
                }
                else //opposite side
                {
                    if (Mathf.Abs(direction.y) > 0)
                    {
                        moveTime = 0;
                        myCamera.transform.position += direction;
                        Vector3 rotation = myCamera.transform.localEulerAngles;
                        myCamera.transform.localEulerAngles = new Vector3(rotation.x + 10 * direction.y, rotation.y, rotation.z);
                        return true;
                    }
                    else
                    {
                        moveTime = 0;
                        myCamera.transform.position += direction;
                        return true;
                    }
                }
                
            }
        }
        return false;
    }
}
