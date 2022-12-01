using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButtonScript : MonoBehaviour
{
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject Tutorial1;
    [SerializeField] GameObject Tutorial2;
    [SerializeField] GameObject Tutorial3;
    [SerializeField] GameObject Credits;
    private int spot = 0;

    private void OnEnable()
    {
        Tutorial1.SetActive(true);
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(false);
        Credits.SetActive(false);
    }

    public void OnClick()
    {
        spot++;
        if (spot == 1)
        {
            Tutorial1.SetActive(false);
            Tutorial2.SetActive(true);
        }
        else if (spot == 2)
        {
            Tutorial2.SetActive(false);
            Tutorial3.SetActive(true);
        }
        else if (spot == 3)
        {
            Tutorial3.SetActive(false);
            Credits.SetActive(true);
        }
        else if (spot == 4)
        {
            spot = 0;
            Credits.SetActive(false);
            TutorialPanel.SetActive(false);
        }
    }
}
