using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{

    public GameObject Wizar;
    public GameObject Warroir;
    public GameObject Crow;

    enum AS { //Action States
        Wizar,
        WArrior,
        Crow,
        None
    }

    public GameObject CurrentCharacter;

    AS State = AS.None;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentCharacter) {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Grid.Map[0, 0] == null) {
                Transform t = CurrentCharacter.GetComponent<Transform>();
                int x = (int)t.position.x;
                int y = (int)t.position.y;
                t.position = new Vector2(0f, 0f);
                Grid.UpdateMap(CurrentCharacter, x, y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Grid.Map[0, 1] == null) {
                Transform t = CurrentCharacter.GetComponent<Transform>();
                int x = (int)t.position.x;
                int y = (int)t.position.y;
                t.position = new Vector2(0f, 1f);
                Grid.UpdateMap(CurrentCharacter, x, y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Grid.Map[0, 2] == null) {
                Transform t = CurrentCharacter.GetComponent<Transform>();
                int x = (int)t.position.x;
                int y = (int)t.position.y;
                t.position = new Vector2(0f, 2f);
                Grid.UpdateMap(CurrentCharacter, x, y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && Grid.Map[0, 3] == null) {
                Transform t = CurrentCharacter.GetComponent<Transform>();
                int x = (int)t.position.x;
                int y = (int)t.position.y;
                t.position = new Vector2(0f, 3f);
                Grid.UpdateMap(CurrentCharacter, x, y);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && Grid.Map[0, 4] == null) {
                Transform t = CurrentCharacter.GetComponent<Transform>();
                int x = (int)t.position.x;
                int y = (int)t.position.y;
                t.position = new Vector2(0f, 4f);
                Grid.UpdateMap(CurrentCharacter, x, y);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {

            }
            if (Input.GetKeyDown(KeyCode.E)) {
                CurrentCharacter.GetComponent<Player>().EMove.Use(CurrentCharacter);
            }
        }
    }

    public void NoneClick() {
        CurrentCharacter = null;
        State = AS.None;
    }

   public void WizarClick() {
        CurrentCharacter = Wizar;
        State = AS.Wizar;
    }

    public void WArroirClick() {
        CurrentCharacter = Warroir;
        State = AS.WArrior;
    }

    public void CrowClick() {
        CurrentCharacter = Crow;
        State = AS.Crow;
    }
}
