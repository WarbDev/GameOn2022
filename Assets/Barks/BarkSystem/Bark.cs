using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Bark : MonoBehaviour
{

    [SerializeField] List<AudioClip> Clips;
    [SerializeField] List<string> Words;
    [SerializeField] GameObject TextBox; //This TextBox is the child of gameObject
    [SerializeField] float DurationInSeconds;
    [SerializeField] bool isBuff;
    private GameObject instance;

    public bool finishedAnimation = false;
    public Vector3 diff;
    public GameObject player;

    Canvas canvas;
    public Camera cameraa;

    private int barkNumber = 0;

    public void setBark(Camera camera)
    {
        //cameraa = camera;
    }


    public void PlayBark(GameObject player, Canvas canvas, Camera camera)
    {
        this.enabled = true;
        this.canvas = canvas;
        this.player = player;
        cameraa = camera;



        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);

        SetText(player);
    }

    private void SetText(GameObject player)
    {

        string saying;
        if (barkNumber == 0 && isBuff)
        {
            saying = "The reason why I'm so strong is simple.";
        } 
        else if (barkNumber == 1 && isBuff)
        {
            saying = "I do 100 push-ups every day";
        }
        else if (barkNumber == 2 && isBuff)
        {
            saying = "and eat a high-protein diet invented by top bodybuilders";
        }
        else if (barkNumber == 3 && isBuff)
        {
            saying = "this diet consists of, but is not limited to:";
        }
        else
        {
            int randNumber = Random.Range(0, Words.Count);
            saying = Words[randNumber];
        }


        TextBox.GetComponent<TextMeshProUGUI>().SetText(saying);
        Bark instance = Instantiate(this);
        Destroy(instance.gameObject, DurationInSeconds);

        instance.player = player;


        //instance.cameraa = cameraa;

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = player.transform.position + new Vector3(0, 1f);

        



        RectTransform transform = instance.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(1 + Mathf.Sqrt(saying.Length)* 1f, 1 + Mathf.Sqrt(saying.Length)* .15f);

        instance.transform.DOScale(0, 1).From();

        if (Random.Range(0f, 1f) > .5)
        {
            instance.diff = new Vector3(-(.5f + transform.sizeDelta.x / 2), 1);
        }
        else
        {
            instance.diff = new Vector3(.5f + transform.sizeDelta.x / 2, 1);
        }

        instance.transform.DOMove(player.transform.position + instance.diff, 1).OnComplete(() => instance.finishedAnimation = true);

        barkNumber++;
    }



    private void Update()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        if (finishedAnimation)
        {
             transform.SetPositionAndRotation(player.transform.position + diff, new Quaternion(-Mathf.Abs(cameraa.transform.rotation.x), transform.rotation.y, transform.rotation.z, transform.rotation.w));
        }
        else
        {
            transform.SetPositionAndRotation(transform.position, new Quaternion(-Mathf.Abs(cameraa.transform.rotation.x), transform.rotation.y, transform.rotation.z, transform.rotation.w));
        }
        
    }

}
