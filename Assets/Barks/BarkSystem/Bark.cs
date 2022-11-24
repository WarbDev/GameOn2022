using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//[CreateAssetMenu(menuName = "Bark")]
public class Bark : MonoBehaviour
{

    [SerializeField] List<AudioClip> Clips;
    [SerializeField] List<string> Words;
    [SerializeField] GameObject TextBox;

    public void PlayBark(GameObject player)
    {
        int randNumber = Random.Range(1, Clips.Count);
        GlobalAudioSource.Instance.Play(Clips[randNumber]);
        randNumber = Random.Range(1, Words.Count);
        gameObject.GetComponent<TextMeshPro>().SetText(Words[randNumber]);
        GameObject textObject = Instantiate(gameObject);

        textObject.transform.SetParent(WorldCanvasSingleton.Instance.transform, false);

        textObject.transform.position = player.transform.position;

    }
}
