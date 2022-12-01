using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkManager : MonoBehaviour
{

    [SerializeField] BarkPlayerComponant bufBark;
    [SerializeField] BarkPlayerComponant spyBark;
    [SerializeField] BarkPlayerComponant wizBark;

    [SerializeField] Canvas canvas;
    [SerializeField] Camera cameraa;

    [SerializeField] bool alwaysPlay;

    // Start is called before the first frame update
    private void Start()
    {
        GameFlow.NewPlayerPhase += onTurnBegin;

        bufBark.setBark(cameraa);
        spyBark.setBark(cameraa);
        wizBark.setBark(cameraa);
    }

    private void onTurnBegin()
    {

        float rand = Random.Range(0f, 1f);
        if (rand > .70f)
        {
            rand = Random.Range(0f, 1f);
            if (rand < .45f)
            {
                bufBark.PlayMyBark(canvas, cameraa);
            }
            else if (rand > .80f)
            {
                spyBark.PlayMyBark(canvas, cameraa);
            }
            else
            {
                wizBark.PlayMyBark(canvas, cameraa);
            }
        }
        
    }

    private void OnDestroy()
    {
        GameFlow.NewPlayerPhase -= onTurnBegin;
    }

}
