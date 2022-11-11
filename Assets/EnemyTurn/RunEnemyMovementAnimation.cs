using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RunEnemyMovementAnimation : MonoBehaviour
{
    [SerializeField] RunEnemyMovement enemyTurn;
    
    bool isPlaying = false;
    public bool IsPlaying { get => isPlaying; }

    private void Awake()
    {
        enemyTurn.CalculatedEnemiesMovement += PrepareMovement;
    }

    public void Play()
    {
        isPlaying = true;
    }

    void PrepareMovement(List<MoveLog> moveLogs)
    {
        StartCoroutine(DoMovement(moveLogs));
    }

    IEnumerator DoMovement(List<MoveLog> moveLogs)
    {
        while (!isPlaying)
        {
            yield return null;
        }

        foreach (var log in moveLogs)
        {
            Enemy enemy;
            var hasEnemy = LocationUtility.TryGetEnemy(log.End, out enemy);
            if (hasEnemy)
            {
                enemy.GetComponent<AnimatableEntity>()
                    .PlayAnimation(ANIMATION_ID.ENTITY_JUMP, 
                    new JumpAnimationProperties(LocationUtility.LocationToVector3(log.Start), LocationUtility.LocationToVector3(log.End)));
            }
            yield return new WaitForSeconds(0.1f);
        }
        isPlaying = false;
    }
}
