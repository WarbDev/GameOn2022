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
    private GameObject instance;


    Canvas canvas;
    public Camera cameraa;

    public void setBark(Camera camera)
    {
        //cameraa = camera;
    }


    public void PlayBark(GameObject player, Canvas canvas, Camera camera)
    {
        this.enabled = true;
        this.canvas = canvas;
        cameraa = camera;



        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);

        SetText(player);
    }

    private void SetText(GameObject player)
    {


        int randNumber = Random.Range(0, Words.Count - 1);
        string saying = Words[randNumber];
        TextBox.GetComponent<TextMeshProUGUI>().SetText(saying);
        Debug.Log(saying);

        Bark instance = Instantiate(this);
        Destroy(instance.gameObject, DurationInSeconds);



        instance.cameraa = cameraa;

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = player.transform.position + new Vector3(0, 1.5f);

        



        RectTransform transform = instance.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(2 + Mathf.Sqrt(saying.Length)*.2f, 2 + Mathf.Sqrt(saying.Length)*.1f);

        instance.transform.DOScale(0, 1).From();

        Vector3 to = new();

        if (Random.Range(0f, 1f) > .5)
        {
            to = player.transform.position + new Vector3(-1, 1);
        }
        else
        {
            to = player.transform.position + new Vector3(1, 1);
        }

        instance.transform.DOMove(to, 1);

    }

    private void Update()
    {
        transform.SetPositionAndRotation(transform.position, new Quaternion(-Mathf.Abs(cameraa.transform.rotation.x), transform.rotation.y, transform.rotation.z, transform.rotation.w));
    }

}
