using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoralButtonScript : MonoBehaviour
{
    [SerializeField] GameObject TutorialPanel;
    public void OnClick()
    {
        TutorialPanel.SetActive(true);
    }
}
