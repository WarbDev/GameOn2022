using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RunEnemyMovementAnimation : MonoBehaviour
{
    [SerializeField] RunEnemyMovement enemyTurn;
    [SerializeField] Ease ease;
    [SerializeField] float duration;
    [SerializeField] float jumpHeight;

    private void Awake()
    {
        enemyTurn.CalculatedEnemiesMovement += ShowMovement;
    }

    void ShowMovement(List<MoveLog> moveLogs)
    {
        StartCoroutine(DoMovement(moveLogs));
    }

    IEnumerator DoMovement(List<MoveLog> moveLogs)
    {
        
        foreach (var log in moveLogs)
        {
            Enemy enemy;
            var hasEnemy = LocationUtility.TryGetEnemy(log.End, out enemy);
            if (hasEnemy)
            {
                var transform = enemy.gameObject.transform;

                Sequence mySequence = DOTween.Sequence();
                mySequence.SetEase(ease);
                mySequence.Append(transform.DOMove(new Vector3((log.End.X + log.Start.X) / 2f, log.End.Y + jumpHeight), duration / 2));
                mySequence.Append(transform.DOMove(new Vector3(log.End.X, log.End.Y), duration / 2));
            }
            yield return new WaitForSeconds(0.1f);
        }
        StopAllCoroutines();
    }
}
