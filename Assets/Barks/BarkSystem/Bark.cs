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
    Camera cameraa;

    public void setBark(Camera camera)
    {
        cameraa = camera;
    }


    public void PlayBark(GameObject player, Canvas canvas)
    {
        this.canvas = canvas;
        

        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);

        SetText(player);
    }

    private void SetText(GameObject player)
    {

        instance = Instantiate(gameObject);
        
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = player.transform.position;

        Destroy(instance, DurationInSeconds);

        int randNumber = Random.Range(0, Words.Count);
        string saying = Words[randNumber];

        TextBox.GetComponent<TextMeshProUGUI>().SetText(saying);

        RectTransform transform = instance.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(2 + Mathf.Sqrt(saying.Length)*.2f, 2 + Mathf.Sqrt(saying.Length)*.1f);


        

        

        SpriteRenderer textBoxRenderer = TextBox.GetComponent<SpriteRenderer>();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        instance.transform.DOScale(0, DurationInSeconds).From();

        Vector3 to = player.transform.position + new Vector3(1, 1);

        instance.transform.DOMove(to, DurationInSeconds);

        //rectTransform.sizeDelta =  new Vector2(textBoxRenderer.bounds.size.x, textBoxRenderer.bounds.size.y);

    }

    private void Update()
    {
        transform.SetPositionAndRotation(transform.position, new Quaternion(-Mathf.Abs(cameraa.transform.rotation.x), transform.rotation.y, transform.rotation.z, transform.rotation.w));
    }

}
