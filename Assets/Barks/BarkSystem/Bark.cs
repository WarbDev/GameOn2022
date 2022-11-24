using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


//[CreateAssetMenu(menuName = "Bark")]
public class Bark : MonoBehaviour
{

    [SerializeField] List<AudioClip> Clips;
    [SerializeField] List<string> Words;
    [SerializeField] GameObject TextBox; //This TextBox is the child of gameObject
    [SerializeField] float DurationInSeconds;
    private GameObject instance;

    public void PlayBark(GameObject player)
    {
        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);

        SetText(player);
    }

    private void SetText(GameObject player)
    {

        instance = Instantiate(gameObject);
        Destroy(instance, DurationInSeconds);

        int randNumber = Random.Range(0, Words.Count);
        instance.GetComponent<TextMeshPro>().SetText(Words[randNumber]);

        instance.transform.SetParent(player.transform, false);

        SpriteRenderer textBoxRenderer = TextBox.GetComponent<SpriteRenderer>();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        instance.transform.DOScale(0, DurationInSeconds).From();

        Vector3 to = instance.transform.position + new Vector3(1, 1);

        instance.transform.DOMove(to, DurationInSeconds);

        rectTransform.sizeDelta = 
            new Vector2(textBoxRenderer.bounds.size.x, textBoxRenderer.bounds.size.y);

    }

}
