using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveBegin : MonoBehaviour
{
    [SerializeField] LevelConstructor levelConstructor;
    [SerializeField] EnemyPhase enemyPhase;
    [SerializeField] PhaseEnd phaseEnd;
    
    public void BeginWave(Wave wave)
    {
        StartCoroutine(WaveBeginRoutine(wave));
    }

    IEnumerator WaveBeginRoutine(Wave wave)
    {
        bool finishedLevelConstruction = false;
        levelConstructor.Finished += FinishedLevelConstruction;
        levelConstructor.Run(wave);
        
        while (!finishedLevelConstruction)
        {
            yield return null;
        }
        levelConstructor.Finished -= FinishedLevelConstruction;

        void FinishedLevelConstruction() => finishedLevelConstruction = true;

        int phasesToDo = 3;
        Action doConsecutivePhases = DoConsecutivePhases;
        enemyPhase.StartRound(doConsecutivePhases);

        void DoConsecutivePhases()
        {
            if (phasesToDo > 0)
            {
                Action moreConsecutivePhases = DoConsecutivePhases;
                enemyPhase.StartRound(moreConsecutivePhases);
                phasesToDo--;
            }
            else
            {
                Action goToPhaseEnd = () => phaseEnd.EndPhase(Phase.ENEMY_PHASE);
                enemyPhase.StartRound(goToPhaseEnd);
            }
        }
    }
}
