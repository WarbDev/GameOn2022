using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class SkipPlayerTurn : MonoBehaviour
{

    [SerializeField] Image myImage;
    private Tween flash;

    public event Action RequestedPlayerSkip;

    private void Start()
    {
        UIPlayerEvents.AnyPlayerNowPlanning += nowPlanning;
        flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
    }

    private void nowPlanning(bool isPlanning)
    {
        if (!isPlanning)
        {
            flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            flash.Kill();
            Color col = myImage.color;
            col.a = .4f;
            myImage.color = col;
        }
    }

    public void SkipPlayerTurns()
    {
        RequestedPlayerSkip?.Invoke();
    }
}
