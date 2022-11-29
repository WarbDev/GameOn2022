using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MoveButton : MonoBehaviour, IBroadcastMove
{
    public event Action<Move> Broadcast;
    [SerializeField] Move move;


    [SerializeField] UIPlayerEvents events;

    [SerializeField] TextMeshProUGUI textMesh;

    private Image myImage;
    private Tween flash;
    private int cooldown = 0;

    public Move Move { get => move; }

    private void Start()
    {

        myImage = gameObject.GetComponent<Image>();
        flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);


        events.ActionNeeded += moveChanged;
        events.CoolDownChanged += onCooldownChange;
        events.StartedPlanningAction += planAction;
    }

    private void planAction(Move move)
    {

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
        if (flash != null)
        {
            flash.Kill();
        }

        Color col = myImage.color;
        col.a = .4f;
        myImage.color = col;
        if (isAvailable && cooldown == 0)
        {
            flash = myImage.DOFade(.75f, .7f).SetLoops(-1, LoopType.Yoyo);
        }

    }
    public void Clicked()
    {
        Broadcast?.Invoke(move);
    }
}