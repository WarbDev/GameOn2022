using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public class WallScript : MonoBehaviour
{
    //[SerializeField] GameObject wallTile;
    //[SerializeField] GameObject wallWindowTile;
    [SerializeField] GameObject pivot;
    [SerializeField] Ease initialEase;
    [SerializeField] Ease wobbleEase;
    [SerializeField] Ease moveEase;

    [SerializeField] GameObject backGroundObject;
    private GameObject backGround;

    [Range(0.25f, 10f)]
    [SerializeField] float liftSpeed = 1f;

    [Range(0.25f, 10f)]
    [SerializeField] float wobbleSpeed = 1f;

    [Range(0.25f, 10f)]
    [SerializeField] float rotateSpeed = 1f;

    public event Action Finished;

    private void Start()
    {
        pivot.transform.position = new Vector3(1000, GameMap.TopBorder + 0.5f, 200);
    }
    public void RaiseWall()
    {
        gameObject.SetActive(true);

        Sequence mySequence = DOTween.Sequence();
        pivot.transform.position = new Vector3(0, GameMap.TopBorder + 0.5f, 200);
        mySequence.Append(pivot.transform.DOMove(new Vector3(0, GameMap.TopBorder + 0.5f, 0), 1f / liftSpeed).SetEase(moveEase));
        

        
        pivot.transform.SetPositionAndRotation(pivot.transform.position, Quaternion.identity);
        Vector3 targetPositionovershoot = new Vector3(-60, 0, 0); // to go from 180 to -60
        Vector3 targetPosition = new Vector3(-45, 0, 0); // to go from -60 to -45


        mySequence.Append(pivot.transform.DORotate(targetPositionovershoot, 1.25f / rotateSpeed).SetEase(initialEase));
        mySequence.Append(pivot.transform.DORotate(targetPosition, wobbleSpeed).SetEase(wobbleEase)).OnComplete(finished);


    }

    private void finished()
    {
        backGround = Instantiate(backGroundObject);
        backGround.transform.position = new Vector3(0, 10, 0);
        backGround.transform.localEulerAngles = new Vector3(-40, 0);

        backGround.GetComponent<SpriteRenderer>().DOFade(0, 1).From();

        Debug.Log("HEHE");

        Finished?.Invoke();
    }

    public void LowerWall()
    {
        Vector3 targetPosition = new Vector3(40, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(gameObject.transform.DORotate(targetPosition, 2f).SetEase(wobbleEase)).OnComplete(deactivate);

        void deactivate()
        {
            int mapHeight = GameMap.TopBorder;
            Vector3 currentPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(currentPosition.x, -5000f, currentPosition.z);

            Destroy(backGround);

            Finished?.Invoke();
        }
    }

    /*private IEnumerator MakeWall()
    {
        MakeWallColumn(0);
        for (int j = 1; j < widthInOneDirection; j++)
        {
            yield return new WaitForSeconds(.2f);
            MakeWallColumn(j);
            MakeWallColumn(-j);
        }
    }

    private void MakeWallColumn(int position)
    {
        for (int i = 0; i < height; i++)
        {
            MakeWallTile(wallTile, new Vector3(position, i));
        }
    }

    private void MakeWallTile(GameObject myTile, Vector3 targetPosition)
    {
        GameObject tile = Instantiate(myTile, gameObject.transform);
        tile.transform.localPosition = new Vector3(targetPosition.x, targetPosition.y);
        var targetPositionovershoot = new Vector3(targetPosition.x, targetPosition.y, -0.3f);

        tile.name = "WallTile (" + targetPosition.x + ", " + targetPosition.y + ")";

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tile.transform.DOMove(targetPositionovershoot, 0.5f));
        mySequence.Append(tile.transform.DOMove(targetPosition, 0.1f));
    }*/
}
