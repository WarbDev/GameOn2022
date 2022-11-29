using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class MovementButton : MonoBehaviour, IBroadcastVoid
{

    [SerializeField] UIPlayerEvents events;

    public event Action Broadcast;
    private Image myImage;
    private Tween flash;

    private void Start()
    {

        myImage = gameObject.GetComponent<Image>();
        flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);


        events.MovementNeeded += moveChanged;
    }

    private void moveChanged(bool isAvailable)
    {
        if (flash != null)
        {
            flash.Kill();
        }

        Color col = myImage.color;
        col.a = .4f;
        myImage.color = col;
        if (isAvailable)
        {
            flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
        }

    }

    public void Clicked()
    {
        Broadcast?.Invoke();
    }
}