using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkManager : MonoBehaviour
{

    [SerializeField] BarkPlayerComponant bufBark;
    [SerializeField] BarkPlayerComponant spyBark;
    [SerializeField] BarkPlayerComponant wizBark;

    [SerializeField] Canvas canvas;

    // Start is called before the first frame update
    private void Start()
    {
        GameFlow.NewPlayerPhase += onTurnBegin;
    }

    private void onTurnBegin()
    {
        float rand = Random.Range(0f, 1f);

        if (rand < .33)
        {
            bufBark.PlayMyBark(canvas);
        } 
        else if (rand > .66)
        {
            spyBark.PlayMyBark(canvas);
        }
        else
        {
            wizBark.PlayMyBark(canvas);
        }
    }

    private void OnDestroy()
    {
        GameFlow.NewPlayerPhase -= onTurnBegin;
    }

}
