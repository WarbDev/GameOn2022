using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class WaveWin : MonoBehaviour
{

    [SerializeField] GameObject playerUi;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] List<GameObject> children;

    public event Action onWinScreenDismissed;

    private int waveNumber;

    //wavenumber -1 means the whole game has been won
    public void WaveWon(int waveNumber)
    {
        this.waveNumber = waveNumber;
        

        if (waveNumber == -1)
        {
            gameObject.SetActive(true);
            textMesh.text = "And so the line was held";
        }
        else
        {
            playerUi.transform.DOScaleY(0f, 1f).SetEase(Ease.InBack).OnComplete(afterUiShrinks);
        }
    }

    private void afterUiShrinks()
    {
        gameObject.SetActive(true);
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        gameObject.GetComponent<Image>().DOFade(0, 3).From().SetEase(Ease.OutBack).OnComplete(afterExists);

    }

    private void afterExists()
    {
        foreach (GameObject child in children)
        {
            child.SetActive(true);
        }
        textMesh.text = "WAVE " + waveNumber + " COMPLETE!";
    }
}
