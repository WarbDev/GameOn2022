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

    private bool isAvailable = true;

    private void Start()
    {

        myImage = gameObject.GetComponent<Image>();
        flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);


        events.MovementNeeded += moveChanged;
        events.StartedPlanningAction += planAction;
        events.StartedPlanningMovement += planMovement;

        events.PlannedActionSuccessful += actionSuccess;
        events.PlannedMovementSuccessful += moveSuccess;
    }

    private void actionSuccess(Move move, bool actionSuccessful)
    {
        if (isAvailable)
        {
            //Color col = myImage.color;
            //col.a = .4f;
            //myImage.color = col;
            flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
        }
        

    }

    private void moveSuccess(bool moveSuccessful)
    {
        if (isAvailable)
        {
            //Color col = myImage.color;
            //col.a = .4f;
            //myImage.color = col;
            flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void planMovement()
    {
        if (flash != null)
        {
            flash.Kill();
        }
        Color col = myImage.color;
        col.a = .4f;
        myImage.color = col;
    }

    private void planAction(Move move)
    {
        if (flash != null)
        {
            flash.Kill();
        }
        Color col = myImage.color;
        col.a = .4f;
        myImage.color = col;
    }


    //THIS IS NOT CALLED WHEN THE PLAYER'S OWN MOVES BECOME AVAILABLE OR UNAVAILABLE
    //PERHAPS DEBUG?
    private void moveChanged(bool isAvailable)
    {
        this.isAvailable = isAvailable;
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