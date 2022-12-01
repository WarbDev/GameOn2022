using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkPlayerComponant : MonoBehaviour
{
    [SerializeField] Bark talkBark;

    public void setBark(Camera camera)
    {
        talkBark.setBark(camera);
    }

    public void PlayMyBark(Canvas canvas, Camera camera)
    {
        talkBark.PlayBark(gameObject, canvas, camera);
    }
}
