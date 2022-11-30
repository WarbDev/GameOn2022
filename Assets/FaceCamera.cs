using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Transform parentTransform = null;
    [SerializeField] Transform transformToAdjust = null;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    public SpriteRenderer Sprite { get => spriteRenderer; }
    public Transform ParentTransform { get => parentTransform; set => parentTransform = value; }

    int i = 0;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.rendererPriority = 100 - Mathf.FloorToInt(parentTransform.position.y + .5f);
        }
        
        transformToAdjust.SetPositionAndRotation(AdjustPositionBasedOnCameraAngle(), new Quaternion(-Mathf.Abs(mainCamera.transform.rotation.x), transformToAdjust.rotation.y, transformToAdjust.rotation.z, transformToAdjust.rotation.w));
    }

    Vector3 AdjustPositionBasedOnCameraAngle()
    {
        float adjustedEulerX = mainCamera.transform.eulerAngles.x;
        if (adjustedEulerX >= 270)
            adjustedEulerX -= 360;


        float adjustedY = adjustedEulerX/1000 -.3f;
        Vector3 vec = new Vector3(parentTransform.position.x, parentTransform.position.y + adjustedY, parentTransform.position.z);

        return vec;
    }


}
