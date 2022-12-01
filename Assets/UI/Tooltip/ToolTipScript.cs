using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponant;
    [SerializeField] RectTransform myRectTransform;
    [SerializeField] Camera myCamera;
    private RectTransform parentRectTransform;

    private static ToolTipScript instance;

    private void Awake()
    {
        instance = this;
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        HideToolTip();
    }

    //private void Start()
    //{
    //    ShowToolTip("Punch: fool fool fool fishing fooolish fool fool FOOOOOOOOOOOOOOOOOOOOL WHIIPSERSNAPPER");
    //}

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, myCamera, out localPoint);
        transform.localPosition = localPoint;
        if (localPoint.x < 200)
        {
            myRectTransform.pivot = new Vector2(0, 0);
        }
        else
        {
            myRectTransform.pivot = new Vector2(1, 0);
        }
    }

    private void ShowToolTip(string text)
    {
        gameObject.SetActive(true);
        text = text.Replace("\\n", "\n");
        textComponant.text = text;
        Vector2 size = new Vector2(Mathf.Sqrt(text.Length) * 35f, 50 + Mathf.Sqrt(text.Length) * 12f); //new Vector2(textComponant.preferredWidth*.5f + padding*2, textComponant.preferredHeight + padding * 2);

        myRectTransform.sizeDelta = size;

    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string tooltipString)
    {
        instance.ShowToolTip(tooltipString);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }
}
