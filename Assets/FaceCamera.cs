using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Transform transformToAdjust;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.rendererPriority = 100 - Mathf.FloorToInt(transform.position.y + .5f);
        transformToAdjust.SetPositionAndRotation(AdjustPositionBasedOnCameraAngle(), new Quaternion(mainCamera.transform.rotation.x, transformToAdjust.rotation.y, transformToAdjust.rotation.z, transformToAdjust.rotation.w));
    }

    Vector2 AdjustPositionBasedOnCameraAngle()
    {
        float adjustedEulerX = mainCamera.transform.eulerAngles.x;
        if (adjustedEulerX >= 270)
            adjustedEulerX -= 360;


        float adjustedY = adjustedEulerX/100;
        Vector2 vec = new Vector2(transform.position.x, transform.position.y + adjustedY);

        return vec;
    }


}
