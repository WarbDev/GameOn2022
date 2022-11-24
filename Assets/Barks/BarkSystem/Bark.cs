using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//[CreateAssetMenu(menuName = "Bark")]
public class Bark : MonoBehaviour
{

    [SerializeField] List<AudioClip> Clips;
    [SerializeField] List<string> Words;
    [SerializeField] GameObject TextBox; //This TextBox is the child of gameObject

    public void PlayBark(GameObject player)
    {
        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);

        SetText(player);
    }

    private void SetText(GameObject player)
    {

        int randNumber = Random.Range(1, Words.Count);
        gameObject.GetComponent<TextMeshPro>().SetText(Words[randNumber]);

        GameObject textObject = Instantiate(gameObject);
        textObject.transform.SetParent(player.transform, false);

        SpriteRenderer textBoxRenderer = TextBox.GetComponent<SpriteRenderer>();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = 
            new Vector2(textBoxRenderer.bounds.size.x, textBoxRenderer.bounds.size.y);



    }
}
