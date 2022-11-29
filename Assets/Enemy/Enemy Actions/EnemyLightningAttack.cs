using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyLightningAttack : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    [SerializeField] protected float damage;
    [SerializeField] protected int range;

    public int Range { get => range; }

    public override void DoEnemyAction()
    {

    }
}