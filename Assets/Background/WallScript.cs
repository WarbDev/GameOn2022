using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WallScript : MonoBehaviour
{
    //[SerializeField] GameObject wallTile;
    //[SerializeField] GameObject wallWindowTile;
    [SerializeField] Ease initialEase;
    [SerializeField] Ease wobbleEase;
    private int mapHeight;

    public void RaiseWall()
    {
        int mapHeight = GameMap.TopBorder;
        Vector3 currentPosition = gameObject.transform.position;
        gameObject.transform.position = new Vector3(currentPosition.x, mapHeight + .85f, currentPosition.z);

        gameObject.SetActive(true);
        Vector3 targetPositionovershoot = new Vector3(-60, 0);
        Vector3 targetPosition = new Vector3(-45, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(gameObject.transform.DORotate(targetPositionovershoot, 2f).SetEase(initialEase));
        mySequence.Append(gameObject.transform.DORotate(targetPosition, 2f).SetEase(wobbleEase));
        
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
        }
    }

    private void MapHeightExpanded()
    {
        
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
