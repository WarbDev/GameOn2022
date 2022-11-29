using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour, IBroadcastMove
{
    public event Action<Move> Broadcast;
    [SerializeField] Move move;

    [SerializeField] UIPlayerEvents events;

    private Image myImage;
    private Tween flash;

    private void Start()
    {

        myImage = gameObject.GetComponent<Image>();
        flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);


        events.ActionNeeded += moveChanged;
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
        Broadcast?.Invoke(move);
    }
}