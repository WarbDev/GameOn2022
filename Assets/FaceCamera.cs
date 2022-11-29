using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform transformToAdjust;
    [SerializeField] SpriteRenderer spriteRenderer;

    int i = 0;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.rendererPriority = 100 - Mathf.FloorToInt(parentTransform.position.y + .5f);
        transformToAdjust.SetPositionAndRotation(AdjustPositionBasedOnCameraAngle(), new Quaternion(mainCamera.transform.rotation.x, transformToAdjust.rotation.y, transformToAdjust.rotation.z, transformToAdjust.rotation.w));
    }

    Vector3 AdjustPositionBasedOnCameraAngle()
    {
        float adjustedEulerX = mainCamera.transform.eulerAngles.x;
        if (adjustedEulerX >= 270)
            adjustedEulerX -= 360;


        float adjustedY = adjustedEulerX/100;
        Vector3 vec = new Vector3(parentTransform.position.x, parentTransform.position.y + adjustedY, parentTransform.position.z);

        return vec;
    }


}
