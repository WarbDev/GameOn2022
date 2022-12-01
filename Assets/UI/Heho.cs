using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heho : MonoBehaviour
{
    [SerializeField] Sprite shortBeard;
    [SerializeField] Sprite longBeard;
    [SerializeField] Image image;

    private bool isLong = true;

    public void OnClick()
    {
        if (isLong)
        {
            isLong = false;
            image.sprite = shortBeard;
        }
        else
        {
            isLong = true;
            image.sprite = longBeard;
        }
    }
}
