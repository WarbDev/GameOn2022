using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MoveButton : MonoBehaviour, IBroadcastMove, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<Move> Broadcast;
    [SerializeField] Move move;


    [SerializeField] UIPlayerEvents events;

    [SerializeField] TextMeshProUGUI textMesh;

    private Image myImage;
    private Tween flash;
    private int cooldown = 0;

    private bool isAvailable = true;

    public Move Move { get => move; }

    private void Start()
    {
        myImage = gameObject.GetComponent<Image>();
        flash = myImage.DOFade(.85f, .7f).SetLoops(-1, LoopType.Yoyo);


        events.ActionNeeded += moveChanged;
        events.CoolDownChanged += onCooldownChange;
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
            flash = myImage.DOFade(.85f, .7f).SetLoops(-1, LoopType.Yoyo);
        }
        
    }

    private void moveSuccess(bool moveSuccessful)
    {
        if (isAvailable)
        {
            //Color col = myImage.color;
            //col.a = .4f;
            //myImage.color = col;
            flash = myImage.DOFade(.85f, .7f).SetLoops(-1, LoopType.Yoyo);
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

    private void onCooldownChange(Move move, int cooldown)
    {
        if (move == this.move)
        {
            this.cooldown = cooldown;
            if (cooldown == 0)
            {
                textMesh.text = "";
            }
            else
            {
                textMesh.text = "" + cooldown;
            }
        }
    }

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
        if (isAvailable && cooldown == 0)
        {
            flash = myImage.DOFade(.85f, .7f).SetLoops(-1, LoopType.Yoyo);
        }

    }
    public void Clicked()
    {
        Broadcast?.Invoke(move);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipScript.ShowToolTip_Static(move.Name + "\n" + move.Tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipScript.HideToolTip_Static();
    }
}