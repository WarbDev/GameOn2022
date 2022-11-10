using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class A_Fireball : MonoBehaviour
{
    [SerializeField] Ease projectileEase;

    private Vector3 endPoint;

    public void PlayAnimation(Vector3 endPoint)
    {
        this.endPoint = endPoint;
        transform.DOMove(endPoint, 1).SetEase(projectileEase).OnComplete(Boom);
    }

    private void Boom()
    {
    }

}
