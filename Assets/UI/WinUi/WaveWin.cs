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
    [SerializeField] GameObject button;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] List<GameObject> children;

    public event Action onWinScreenDismissed;

    private int waveNumber;

    private void Start()
    {
        button.GetComponent<RoundEndButton>().Click += onClick;

        foreach (GameObject child in children)
        {
            
        }
    }

    private void onClick()
    {
        foreach (GameObject child in children)
        {
            child.transform.DOScale(0, 1);  //scales all the buttons to nothing
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.1f);
        gameObject.GetComponent<Image>().DOFade(0, 3).SetEase(Ease.OutBack).OnComplete(FinishedFading); //fades image's alpha to nothing
    }

    private void FinishedFading()
    {
        gameObject.SetActive(false);

        playerUi.transform.DOScaleY(1f, 1f).SetEase(Ease.OutBack); //scales in player UI
        onWinScreenDismissed?.Invoke();


        //reinitializes variables

        foreach (GameObject child in children)
        {
            child.transform.localScale = new Vector3(1, 1, 1);  //scales all the buttons to nothing
        }

        Image image = gameObject.GetComponent<Image>();
        Color color = image.color;
        color.a = 1;
        image.color = color;
    }




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
            playerUi.transform.DOScaleY(0f, 1f).SetEase(Ease.InBack).OnComplete(afterUiShrinks); // shrinks UI
        }
    }

    private void afterUiShrinks()
    {
        gameObject.SetActive(true);
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        gameObject.GetComponent<Image>().DOFade(0, 3).From().SetEase(Ease.OutBack).OnComplete(afterExists); // fade from black to white. Assumes that it is currently white

    }

    private void afterExists()
    {
        foreach (GameObject child in children)
        {
            child.SetActive(true);
            child.transform.DOScale(0, 1).From(); //fades alpha from 0 to 1. assumes that it is currently at full alpha
        }
        textMesh.text = "WAVE " + waveNumber + " COMPLETE!";
    }
}
