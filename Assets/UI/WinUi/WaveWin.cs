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

    [SerializeField] GameObject lostButton;

    [SerializeField] GameObject Tutorial1;
    [SerializeField] GameObject Tutorial2;
    [SerializeField] GameObject Tutorial3;
    [SerializeField] GameObject Tutorial4;
    [SerializeField] GameObject Tutorial5;
    [SerializeField] GameObject Hint1;
    [SerializeField] GameObject Hint2;
    [SerializeField] GameObject Hint3;
    [SerializeField] GameObject Hint4;

    public event Action onWinScreenDismissed;

    public event Action RestartWave;

    private int waveNumber;

    public void Restart()
    {
        RestartWave?.Invoke();
    }

    private void Start()
    {
        button.GetComponent<RoundEndButton>().Click += onClick;
    }

    public void onClick()
    {
        foreach (GameObject child in children)
        {
            child.transform.DOScale(0, 1);  //scales all the buttons to nothing
        }

        if (waveNumber == 2)
        {
            Tutorial1.transform.DOMoveY(from, length);
        }
        if (waveNumber == 3)
        {
            Tutorial2.transform.DOMoveY(from, length);
        }
        if (waveNumber == 4)
        {
            Tutorial3.transform.DOMoveY(from, length);
        }
        if (waveNumber == 6)
        {
            Tutorial4.transform.DOMoveY(from, length);
        }
        if (waveNumber == 8)
        {
            Tutorial5.transform.DOMoveY(from, length);
        }
        if (waveNumber == 1)
        {
            Hint1.transform.DOMoveY(from, length);
        }
        if (waveNumber == 5)
        {
            Hint2.transform.DOMoveY(from, length);
        }
        if (waveNumber == 7)
        {
            Hint3.transform.DOMoveY(from, length);
        }
        if (waveNumber == 9)
        {
            Hint4.transform.DOMoveY(from, length);
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
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(false);
        Tutorial4.SetActive(false);
        Tutorial5.SetActive(false);
        Hint1.SetActive(false);
        Hint2.SetActive(false);
        Hint3.SetActive(false);
        Hint4.SetActive(false);

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
            playerUi.transform.DOScaleY(0f, 1f).SetEase(Ease.InBack).OnComplete(final);
            
        }
        else if (waveNumber == 0)
        {
            playerUi.transform.DOScaleY(0f, 1f).SetEase(Ease.InBack).OnComplete(waveLost); // shrinks UI
        }
        else
        {
            playerUi.transform.DOScaleY(0f, 1f).SetEase(Ease.InBack).OnComplete(afterUiShrinks); // shrinks UI
        }
    }

    private void final()
    {
        gameObject.SetActive(true);
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        gameObject.GetComponent<Image>().DOFade(0, 10).SetEase(Ease.OutBack).From().OnComplete(next);

        
        void next()
        {
            textMesh.gameObject.SetActive(true);
            textMesh.text = "The Line Was Held";
            textMesh.fontSize = 100;
            Vector3 pos = textMesh.gameObject.transform.localPosition;
            textMesh.gameObject.transform.localPosition = new Vector3(pos.x, 200);
            textMesh.transform.DOScale(0, 1).From();
            button.SetActive(false);
        }
    }

    private void waveLost()
    {
        gameObject.SetActive(true);
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        gameObject.GetComponent<Image>().DOFade(0, 3).SetEase(Ease.OutBack).OnComplete(afterExists2); // fade from white to black. Assumes that it is currently white
    }

    private void afterExists2()
    {
        textMesh.gameObject.SetActive(true);
        lostButton.gameObject.SetActive(true);
        lostButton.transform.localScale = new Vector3(1, 1, 1);


        textMesh.transform.DOScale(0, 1).From();
        lostButton.transform.DOScale(0, 1).From();

        textMesh.text = "  GAME OVER";
    }

    private void afterUiShrinks()
    {
        gameObject.SetActive(true);
        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
        lostButton.transform.DOScale(0, 1);
        gameObject.GetComponent<Image>().DOFade(0, 3).From().SetEase(Ease.OutBack).OnComplete(afterExists); // fade from black to white. Assumes that it is currently white

    }

    private int from = -150;
    private float length = 1f;

    private void afterExists()
    {
        foreach (GameObject child in children)
        {
            child.SetActive(true);
            child.transform.DOScale(0, 1).From(); //fades alpha from 0 to 1. assumes that it is currently at full alpha
        }
        textMesh.text = "WAVE " + waveNumber + " COMPLETE!";

        if (waveNumber == 2)
        {
            Tutorial1.SetActive(true);
            Tutorial1.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 3)
        {
            Tutorial2.SetActive(true);
            Tutorial2.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 4)
        {
            Tutorial3.SetActive(true);
            Tutorial3.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 6)
        {
            Tutorial4.SetActive(true);
            Tutorial4.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 8)
        {
            Tutorial5.SetActive(true);
            Tutorial5.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 1)
        {
            Hint1.SetActive(true);
            Hint1.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 5)
        {
            Hint2.SetActive(true);
            Hint2.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 7)
        {
            Hint3.SetActive(true);
            Hint3.transform.DOMoveY(from, length).From();
        }
        if (waveNumber == 9)
        {
            Hint4.SetActive(true);
            Hint4.transform.DOMoveY(from, length).From();
        }
    }
}
